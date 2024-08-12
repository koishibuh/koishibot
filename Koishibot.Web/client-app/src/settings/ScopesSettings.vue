<script setup lang="ts">
import { ref } from 'vue';
import scopeData from '@/settings/data/scopesData.json';
import LabelCheckbox from '@/common/label-checkbox/LabelCheckbox.vue';
</script>

<template>
  <!--   <div class="grid grid-cols-2"> -->

  <LabelCheckbox
    :item="item"
    v-for="(item, index) in scopeData"
    :key="index"
    class="p-2 grid grid-cols-2"
  />

  <!--  </div> -->
  <button class="primary-button mb-3 w-1/2">Save</button>
</template>
