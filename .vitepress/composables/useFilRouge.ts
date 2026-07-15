import { ref, watch } from 'vue'

const DEFAULT = 'esport'
const active = ref(DEFAULT)

if (typeof window !== 'undefined') {
  active.value = localStorage.getItem('fil-rouge') ?? DEFAULT
  watch(active, val => localStorage.setItem('fil-rouge', val))
}

export function useFilRouge() {
  return {
    active,
    set: (id: string) => { active.value = id }
  }
}
