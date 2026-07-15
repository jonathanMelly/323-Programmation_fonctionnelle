// .vitepress/theme/index.js
import DefaultTheme from 'vitepress/theme'
import Layout from './Layout.vue'
import FilRougeLink from '../components/FilRougeLink.vue'
import { useFilRouge } from '../composables/useFilRouge'
import { watch, nextTick } from 'vue'
import './fil-rouge.css'

function markSelectedFilRouge(activeId) {
  nextTick(() => {
    document.querySelectorAll('.fr-active-ctx').forEach(el => el.classList.remove('fr-active-ctx'))
    const link = document.querySelector(`.VPSidebar a[href*="/exos/fil-rouge/"][href$="/${activeId}/"]`)
    if (!link) return
    const item = link.closest('.VPSidebarItem')
    if (!item) return
    item.classList.add('fr-active-ctx')
    // Move selected context to first position in its sibling list
    const parent = item.parentElement
    if (parent && parent.firstElementChild !== item) {
      parent.insertBefore(item, parent.firstElementChild)
    }
  })
}

export default {
  extends: DefaultTheme,
  Layout,
  enhanceApp({ app, router }) {
    app.component('FilRougeLink', FilRougeLink)

    if (typeof window !== 'undefined') {
      const { active } = useFilRouge()
      router.onAfterRouteChange = () => markSelectedFilRouge(active.value)
      watch(active, (val) => markSelectedFilRouge(val))
    }
  }
}
