<script setup lang="ts">
import { ref } from 'vue';
import HiddenField from '@/common/hidden-field/HiddenField.vue';

const address = ref<string>('');
const port = ref<string>('');
const password = ref<string>('');

const saveSettings = () => {
  console.log('saved');
};
</script>

<template>
  <form @submit.prevent="saveSettings()" class="flex flex-col gap-2 my-4 w-full">
    <HiddenField field-name="Address" @update="(x) => (address = x)" />

    <div class="flex items-center">
      <label for="port" class="p-2 w-[60px]">Port:</label>
      <input type="text" v-model="port" id="message" class="text-black" />
    </div>

    <HiddenField field-name="Password" @update="(x) => (password = x)" />

    <button class="primary-button">Save</button>
  </form>
</template>
