<script setup lang="ts">
import { ref, onMounted } from 'vue';
import HiddenField from '@/common/hidden-field/HiddenField.vue';
import { useObsStore } from './obs.store';
import { type IObsRequest } from './models/obs-interface';
import {storeToRefs} from "pinia";

const emit = defineEmits<{
  saveClicked: [void]
}>();

const store = useObsStore();

const address = ref<string>('');
const port = ref<string>('');
const password = ref<string>('');

const saveSettings = async () => {
  const request: IObsRequest = {
    websocketUrl: address.value,
    port: port.value,
    password: password.value
  };
  await store.saveSettings(request);
  emit('saveClicked');
};

</script>

<template>
  <form @submit.prevent="saveSettings()" class="flex flex-col gap-2 my-
  4 w-full">
    <HiddenField field-name="Address" @update="(x) => (address = x)" />
    <div class="flex items-center">
      <label for="port" class="p-2 w-1/4">Port:</label>
      <input type="text" v-model="port" id="message" class="text-black w-2/3" />
    </div>

    <HiddenField field-name="Password" @update="(x) => (password = x)" />

    <button class="primary-button">Save</button>
  </form>
</template>