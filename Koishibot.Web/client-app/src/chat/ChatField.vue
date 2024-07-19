<script setup lang="ts">
import http from '@/api/http';
import { ref } from 'vue';

const message = ref();

async function submit() {
  await http.post('/api/chat', { message: message.value });
  message.value = '';
}
</script>

<template>
  <form @submit.prevent="submit" class="flex p-2 justify-between">
    <input class="w-[85%] rounded p-1" type="text" v-model="message" />
    <input class="bg-accent-two rounded-r p-1 w-[15%]" type="button" @click="submit" value="Send" />
  </form>
</template>
