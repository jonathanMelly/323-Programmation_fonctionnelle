<script setup lang="ts">
import { computed } from 'vue'
import { withBase } from 'vitepress'
import { useFilRouge } from '../composables/useFilRouge'

const props = defineProps<{
  ex: string    // path after scenario, e.g. '03-filter/README.md', 'data/cs2.csv'
  label: string
}>()

const { active } = useFilRouge()
const href = computed(() => {
  let ex = props.ex.endsWith('/') ? props.ex + 'README' : props.ex
  ex = ex.replace(/\.md$/, '')
  return withBase(`/exos/fil-rouge/${active.value}/${ex}`)
})
</script>

<template>
  <a :href="href">{{ label }}</a>
</template>
