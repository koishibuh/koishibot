<script setup lang="ts">
import { watch, ref } from 'vue';
import ChatMessage from './ChatMessage.vue';
import { useChatMessageStore } from './chat-message.store';

const store = useChatMessageStore();
const chatMessageLog = ref();

watch(store.chatMessages, () => {
  if (chatMessageLog.value) {
    window.setTimeout(scroll, 0);
  }
});

const isHovering = ref<boolean>(false);

const scroll = () => {
  if (chatMessageLog.value && !isHovering.value) {
    chatMessageLog.value.scrollTop = chatMessageLog.value.scrollHeight;
  }
};

const scrollToBottom = () => {
  if (chatMessageLog.value) {
    chatMessageLog.value.scrollTop = chatMessageLog.value.scrollHeight;
  }
  showButton.value = false;
};

const handleScroll = (event: any) => {
  if (event.target.scrollTop < event.target.scrollHeight - event.target.offsetHeight) {
    showButton.value = true;
  } else {
    showButton.value = false;
  }
};

const showButton = ref<boolean>(false);
</script>

<template>
  <div v-if="showButton" class="flex justify-center pt-5 w-full absolute">
    <button class="w-1/3 bg-accent-two p-2 rounded border-2" @click="scrollToBottom">
      Scroll Down
    </button>
  </div>
  <div
    class="overflow-auto p-1 px-2"
    ref="chatMessageLog"
    @mouseover="isHovering = true"
    @mouseout="isHovering = false"
    @scroll.passive="handleScroll"
  >
    <ChatMessage v-for="(item, index) in store.chatMessages" :key="index" :chat="item" />
  </div>
</template>
