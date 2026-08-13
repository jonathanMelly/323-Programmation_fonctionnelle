<script setup lang="ts">
import { computed } from 'vue'
import { withBase } from 'vitepress'
import { useFilRouge } from '../composables/useFilRouge'

const props = defineProps<{
  ex: string    // path after scenario, e.g. '03-tri-filter/README.md', 'data/cs2.csv'
  label: string
}>()

const { active } = useFilRouge()
const href = computed(() => {
  // Paths ending with README.md are directory indexes — strip to get a clean dir path
  let ex = props.ex.replace(/\/?README\.md$/, '')
  // Strip .md extension from other markdown links
  ex = ex.replace(/\.md$/, '')
  // Files (with extension) keep their path; directories get a trailing slash
  const isFile = /\.[^/]+$/.test(ex)
  return withBase(ex
    ? `/exos/fil-rouge/${active.value}/${ex}${isFile ? '' : '/'}`
    : `/exos/fil-rouge/${active.value}/`)
})
</script>

<template>
  <a :href="href">{{ label }}</a>
</template>
