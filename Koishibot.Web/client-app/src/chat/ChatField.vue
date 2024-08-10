<script setup lang="ts">
import { ref } from 'vue';
import { useChatMessageStore } from './chat-message.store';

const store = useChatMessageStore();
const message = ref();

const sendMessage = async () => {
  await store.sendChatMessage(message.value);
  message.value = '';
};
</script>

<template>
  <form @submit.prevent="sendMessage" class="flex p-2 justify-between">
    <input class="w-[85%] rounded p-1" type="text" v-model="message" />
    <input
      class="bg-accent-two rounded-r p-1 w-[15%]"
      type="button"
      @click="sendMessage"
      value="Send"
    />
  </form>
</template>
