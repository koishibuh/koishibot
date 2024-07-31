<script setup lang="ts">
import { ref } from 'vue';
import http from '@/api/http';
import { useCommandStore } from './command.store';

const commandName = ref('');

const message = ref<unknown | string>();

const store = useCommandStore();
const nametext = ref('');
const aliastext = ref('');
const messagetext = ref('');
const permissionstext = ref('');
const cooldowntext = ref('');

async function sendCommand() {
  await http.post('/api/command/createcommand', {
    Name: nametext.value,
    Alias: aliastext.value,
    Message: messagetext.value,
    Permissions: permissionstext.value,
    Cooldown: parseInt(cooldowntext.value),
    EnableTimer: false,
    TimerFk: null
  });
}

async function createCommandName() {
  try {
    await store.createCommandName(commandName.value.toLowerCase());
    commandName.value = '';
  } catch (error) {
    message.value = (error as Error).message;
  }
}
</script>

<template>
  {{ message }}
  <h1>Create Command Name</h1>
  <div class="border-2 p-2 border-gray-500 rounded">
    <form @submit.prevent="createCommandName" class="flex flex-col gap-2 my-4">
      <label for="commandname">Command Name:</label>
      <input type="text" v-model="commandName" id="commandname" class="text-black" />
      <button class="primary-button">Send</button>
    </form>
  </div>

  <h1>Test Zone</h1>
  <div class="border-2 p-2 border-gray-500 rounded">
    <form @submit.prevent="sendCommand()" class="flex flex-col gap-2 my-4">
      <label for="name">Name:</label>
      <input type="text" v-model="nametext" id="name" class="text-black" />
      <label for="alias">Alias:</label>
      <input type="text" v-model="aliastext" id="alias" class="text-black" />
      <label for="message">Message:</label>
      <input type="text" v-model="messagetext" id="message" class="text-black" />
      <label for="permissions">Permissions:</label>
      <input type="text" v-model="permissionstext" id="permissions" class="text-black" />
      <label for="cooldown">Cooldown In Minutes:</label>
      <input type="text" v-model="cooldowntext" id="cooldown" class="text-black" />
      <button class="primary-button">Send</button>
    </form>
  </div>
</template>
