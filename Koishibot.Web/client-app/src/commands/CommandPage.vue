<script setup lang="ts">
import { ref } from 'vue';
import http from '@/api/http';

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
</script>

<template>
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
