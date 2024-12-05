<script setup lang="ts">
import {ref} from 'vue';
import ToggleButton from '@/common/toggle-button.vue';
import {type IObsAppName, useObsStore} from '@/settings/obs.store';
import type {IObsRequest, IObsSettings} from './models/obs-interface';

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

const obsItem = ref<IObsAppName>({ id: 0, appName: '' });
</script>

<template>
  <ToggleButton
      button1Name="Enable OBS"
      button2Name="Disable OBS"
      :state="settings.connectionStatus"
      @update-state="connectedStateChanged"
  />

  <div class="w-1/2 mt-3 gap-2 flex">
    <button class="primary-button" @click="showModal">Settings</button>
    <button class="primary-button" @click="store.importObsInputs">Import Inputs</button>
  </div>

  <form @submit.prevent="store.updateObsItemAppName(obsItem)" class="flex flex-col">
    <label for="appName">Update App Name</label>
    <div class="flex">
      <input type="text" v-model="obsItem.id" id="obsItemId" class="text-black"/>
      <input type="text" v-model="obsItem.appName" id="appName" class="text-black"/>
      <button class="primary-button">Save</button>
    </div>
  </form>

  <div v-if="store.obsItemList">
    <table>
      <thead>
      <tr>
        <th>Id</th>
        <th>Type</th>
        <th>OBS Name</th>
        <th>App Name</th>
      </tr>
      </thead>
      <tbody>
      <tr v-for="item in store.obsItemList" :key="item.id" class="border-2 border-gray-500">
        <td>{{ item.id }}</td>
        <td>{{ item.type }}</td>
        <td>{{ item.obsName }}</td>
        <td>{{ item.appName }}</td>
      </tr>

      </tbody>
    </table>


    {{ store.obsItemList.length }}
    {{ store.obsItemList }}

  </div>
  <div></div>
</template>