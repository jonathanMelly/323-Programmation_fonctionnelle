import { defineConfig } from 'vitepress'
import { glob } from 'glob'
import path from 'path'
import { readdirSync, statSync, existsSync, writeFileSync, copyFileSync, mkdirSync, readFileSync } from 'fs'
import type MarkdownIt from 'markdown-it'

process.env.VITE_EXTRA_EXTENSIONS = 'docx,pdf,csv'

// Transforme les liens vers fil-rouge/*/<ex>/ en composant <FilRougeLink> dynamique.
// Le markdown reste navigable en dehors de VitePress (lien statique vers le fil rouge par défaut).
function filRougeLinksPlugin(md: MarkdownIt) {
  md.core.ruler.push('fil-rouge-links', (state) => {
    for (const blockToken of state.tokens) {
      if (blockToken.type !== 'inline' || !blockToken.children) continue

      const children = blockToken.children
      let i = 0
      while (i < children.length) {
        const token = children[i]
        if (token.type !== 'link_open') { i++; continue }

        const href = token.attrGet('href') ?? ''
        const match = href.match(/fil-rouge\/[^/]+\/(.*)$/)
        if (!match) { i++; continue }

        const ex = match[1]

        // Collecter le texte jusqu'à link_close
        let label = ''
        let j = i + 1
        while (j < children.length && children[j].type !== 'link_close') {
          if (children[j].type === 'text') label += children[j].content
          j++
        }
        label = label || ex

        // Remplacer link_open + contenu + link_close par le composant Vue
        const component = new state.Token('html_inline', '', 0)
        component.content = `<FilRougeLink ex="${ex}" label="${label.replace(/"/g, '&quot;')}" />`
        children.splice(i, j - i + 1, component)
        // i inchangé — on reparse depuis la même position (maintenant occupée par le composant)
      }
    }
  })
}

const filRougesData = existsSync('exos/fil-rouge')
  ? readdirSync('exos/fil-rouge')
      .filter(d => statSync(`exos/fil-rouge/${d}`).isDirectory())
      .map(id => {
        const base = `exos/fil-rouge/${id}`
        const subdirs = readdirSync(base).filter(d => statSync(`${base}/${d}`).isDirectory())

        // Dirs without README.md or index.md are data dirs — auto-generate a download page
        const dataDirs = subdirs
          .filter(d => !existsSync(`${base}/${d}/README.md`) && !existsSync(`${base}/${d}/index.md`))
          .map(d => ({ name: d, files: readdirSync(`${base}/${d}`).filter(f => statSync(`${base}/${d}/${f}`).isFile()) }))
          .filter(({ files }) => files.length > 0)

        for (const dir of dataDirs) {
          const links = dir.files.map(f => `- <a href="${f}" download>${f}</a>`).join('\n')
          writeFileSync(`${base}/${dir.name}/index.md`, `# ${dir.name}\n\n${links}\n`)
        }

        // After generation, all subdirs with README.md or index.md are exercises
        const exercises = subdirs.filter(d =>
          existsSync(`${base}/${d}/README.md`) || existsSync(`${base}/${d}/index.md`)
        )

        return { id, exercises, dataDirs }
      })
  : []

// https://vitepress.dev/reference/site-config
export default defineConfig({
  title: "ICT-323 Fun",
  description: "Module ICT 323 sur la programmation fonctionnelle",

  markdown: {
    config: (md) => {
      filRougeLinksPlugin(md)
    }
  },

  themeConfig: {
    // https://vitepress.dev/reference/default-theme-config
    nav: [
      { text: 'Home', link: '/' },
      { text: 'Thématiques', link: '/thematiques/01-paradigmes-fonctionnels' }
    ],

    sidebar: [
      {
        text: 'Thématiques',
        collapsed : false,
        items: glob.sync('thematiques/*.md', { posix: true })
          .sort()
          .map(f => {
            const num = path.basename(f).match(/^(\d+)/)?.[1]
            const h1 = readFileSync(f, 'utf-8').match(/^#\s+(.*)/m)?.[1]
            const title = h1 ?? path.basename(f).replace('.md', '')
            return { text: num ? `${num} — ${title}` : title, link: '/' + f.replace('.md', '') }
          })
      },
      {
        text: 'Supports',
        collapsed : true,
        items: glob.sync('supports/**/*.md',{posix:true})
          .map(f => '/' + f)
          .map((file) => ({ text: `${path.basename(file).replace(".md","")}`, link: `${file}` })).reverse()
      },
      {
        text: 'Activités fil rouge',
        collapsed: true,
        items: filRougesData.map(fr => ({
          text: fr.id,
          collapsed: false,
          link: `/exos/fil-rouge/${fr.id}/`,
          items: fr.exercises.map(ex => ({
            text: ex,
            link: `/exos/fil-rouge/${fr.id}/${ex}/`
          }))
        }))
      },
      {
        text: 'Exercices divers',
        collapsed: true,
        items: glob.sync(['exos/*/README.md','exos/*/enoncé.md'],{posix:true})
          .filter(f => !f.startsWith('exos/fil-rouge/'))
          .map(f => {
            const parts = f.split('/')
            const file = parts[parts.length - 1]
            const dir = `/${parts.slice(0, -1).join('/')}/`
            return { text: parts[1], link: file === 'README.md' ? dir : dir + file.replace('.md', '') }
          })
          .reverse()
      },
    ],

    socialLinks: [
      { icon: 'github', link: '{REPO_URL}' }
    ],
    search: {
      provider: 'local'
    }
  },

  ignoreDeadLinks: true,
  base: "/323-Programmation_fonctionnelle/",//for gh pages

  rewrites: {
    'README.md': 'index.md',
    'exos/:name/README.md': 'exos/:name/index.md',
    'exos/fil-rouge/:ctx/README.md': 'exos/fil-rouge/:ctx/index.md',
    'exos/fil-rouge/:ctx/:ex/README.md': 'exos/fil-rouge/:ctx/:ex/index.md',
  },

  buildEnd: async (siteConfig) => {
    const filRougeDir = 'exos/fil-rouge'
    if (!existsSync(filRougeDir)) {
      console.warn('\n[fil-rouge] Dossier exos/fil-rouge/ introuvable — aucune validation\n')
      return
    }

    if (filRougesData.length === 0) {
      console.warn('\n[fil-rouge] Aucun fil rouge trouvé dans exos/fil-rouge/\n')
      return
    }

    // Validate: every exercise subdir must have README.md or index.md
    const errors: string[] = []
    for (const fr of filRougesData) {
      for (const ex of fr.exercises) {
        const hasPage = existsSync(`${filRougeDir}/${fr.id}/${ex}/README.md`)
                     || existsSync(`${filRougeDir}/${fr.id}/${ex}/index.md`)
        if (!hasPage) errors.push(`  ✗ ${filRougeDir}/${fr.id}/${ex}/`)
      }
    }
    if (errors.length) {
      console.error('\n[fil-rouge] Pages manquantes :\n' + errors.join('\n') + '\n')
      process.exit(1)
    }

    // Copy data files to output so they are downloadable in production
    for (const fr of filRougesData) {
      for (const dir of fr.dataDirs) {
        const destDir = path.join(siteConfig.outDir, 'exos/fil-rouge', fr.id, dir.name)
        mkdirSync(destDir, { recursive: true })
        for (const file of dir.files) {
          copyFileSync(`${filRougeDir}/${fr.id}/${dir.name}/${file}`, path.join(destDir, file))
        }
      }
    }

    console.log(`\n[fil-rouge] ${filRougesData.length} fil(s) rouge validé(s) : ${filRougesData.map(f => f.id).join(', ')} ✓\n`)
  }
})
