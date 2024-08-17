<script setup lang="ts">
import ToggleButton from '@/common/toggle-button.vue';
import {useServiceStatusStore} from '@/layout/service-status.store';
import {useStreamElementsStore} from "@/settings/streamelements.store";
import type {Component} from 'vue';
import {ref} from 'vue';

// const props = defineProps<{
//   streamElementsStatus: boolean;
// }>();
//
// const emit = defineEmits<{
//   settingClicked: [string];
// }>();

const store = useServiceStatusStore();
const streamElementsStore = useStreamElementsStore();

const jwtToken = ref<string>('');

const saveJwtToken = async () => {
  await streamElementsStore.saveJwtToken(jwtToken.value);
  jwtToken.value = '';
}

const connectStreamElements = async () => {
  await streamElementsStore.connectStreamElements();
}
</script>

<template>
  <div class="flex flex-col gap-2">
    <label for="jwtToken">Token</label>
    <input type="password" v-model="jwtToken" id="jwtToken" class="text-black"/>
    <button @click="saveJwtToken()" class="w-1/2 primary-button">Save</button>

    <button class="primary-button" @click="connectStreamElements">Connect</button>
  </div>
</template>