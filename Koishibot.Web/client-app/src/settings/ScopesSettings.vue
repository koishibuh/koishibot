<script setup lang="ts">
import { ref } from 'vue';
import scopeData from '@/settings/data/scopesData.json';
import LabelCheckbox from '@/common/label-checkbox/LabelCheckbox.vue';
import type {IObsRequest} from "@/settings/models/obs-interface";
const emit = defineEmits<{
  saveClicked: [void]
}>();

const saveSettings = async () => {
  emit('saveClicked');
};

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
  <button class="primary-button mb-3 w-1/2" @click="saveSettings">Save</button>
</template>