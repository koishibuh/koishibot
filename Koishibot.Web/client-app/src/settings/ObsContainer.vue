<script setup lang="ts">
import { ref } from 'vue';
import ToggleButton from '@/common/toggle-button.vue';
import { useObsStore } from '@/settings/obs.store';
import type { IObsRequest, IObsSettings } from './models/obs-interface';

const props = defineProps<{
  settings: IObsSettings;
}>();

const emit = defineEmits<{
  settingClicked: [string];
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

const showModal = () => {
  emit('settingClicked', 'ObsSettings');
};
</script>

<template>
  <ToggleButton
    button1Name="Enable OBS"
    button2Name="Disable OBS"
    :state="settings.connectionStatus"
    @update-state="connectedStateChanged"
  />

  <div class="w-1/2 mt-3">
    <button class="primary-button" @click="showModal">Settings</button>
  </div>
  <div></div>
</template>
