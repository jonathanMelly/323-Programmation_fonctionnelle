<!-- Main layout: composes VitePress slots with app-specific components -->
<script setup>
import DefaultTheme from 'vitepress/theme'
import FilRougeSelector from '../components/FilRougeSelector.vue'
import PdfDownloadButton from '../components/PdfDownloadButton.vue'

const { Layout } = DefaultTheme
</script>

<template>
  <Layout>
    <template #nav-bar-content-after>
      <FilRougeSelector />
    </template>
    <template #aside-outline-before>
      <PdfDownloadButton />
      <br />
    </template>
  </Layout>
</template>
