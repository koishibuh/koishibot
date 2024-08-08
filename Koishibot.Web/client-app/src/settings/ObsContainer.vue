<script setup lang="ts">
import { ref } from 'vue';
import ToggleButton from '@/common/toggle-button.vue';
import { useObsStore } from '@/settings/obs.store';
import type { IObsRequest, IObsSettings } from './models/obs-interface';
import HiddenField from '@/common/hidden-field/HiddenField.vue';

const props = defineProps<{
  settings: IObsSettings;
}>();

const store = useObsStore();

const address = ref<string>('');
const port = ref<string>('');
const password = ref<string>('');

const connectedStateChanged = (state: boolean) => {
  console.log(state);
  store.updateObsConnection(state);
};

const saveSettings = async () => {
  const settings: IObsRequest = {
    websocketUrl: address.value,
    port: port.value,
    password: password.value
  };
  await store.saveSettings(settings);
};
</script>

<template>
  <ToggleButton
    button1Name="Enable OBS"
    button2Name="Disable OBS"
    :state="settings.connectionStatus"
    @update-state="connectedStateChanged"
  />

  <div>
    <form @submit.prevent="saveSettings()" class="flex flex-col gap-2 my-4 w-1/2">
      <HiddenField field-name="Address" @update="(x) => (address = x)" />

      <div class="flex items-center">
        <label for="port" class="p-2 w-[60px]">Port:</label>
        <input type="text" v-model="port" id="message" class="text-black" />
      </div>

      <HiddenField field-name="Password" @update="(x) => (password = x)" />

      <button class="primary-button">Save</button>
    </form>
  </div>
</template>
