<script setup lang="ts">
import { watch, ref, onMounted, nextTick } from 'vue';
import ChatMessage from './ChatMessage.vue';
import { useChatMessageStore } from './chat-message.store';

const store = useChatMessageStore();
const chatMessageLog = ref();

onMounted(() => {
  chatMessageLog.value.scrollTop = chatMessageLog.value.scrollHeight;
})

const isHovering = ref<boolean>(false);

watch(store.chatMessages, async () => {
  if (!isHovering.value) {
  await nextTick();
  chatMessageLog.value.scrollTop =
      chatMessageLog.value.scrollHeight + chatMessageLog.value.offsetHeight;
  }
});

const showButton = ref<boolean>(false);

const scrollToBottom = () => {
  chatMessageLog.value.scrollTop = chatMessageLog.value.scrollHeight;
};

const handleScroll = (event: any) => {
  showButton.value = event.target.scrollTop < event.target.scrollHeight - 423;
};

</script>

<template>
  <div v-if="showButton" class="flex justify-center pt-5 w-full absolute">
    <button class="w-1/3 bg-accent-two p-2 rounded border-2" @click="scrollToBottom">
      Scroll Down
    </button>
  </div>

  <div class="flex flex-col overflow-y-scroll px-2 min-h-[423px]" ref="chatMessageLog"
       @mouseover="isHovering = true"
       @mouseout="isHovering = false"
       @scroll.passive="handleScroll">
    <ChatMessage v-for="(item, index) in store.chatMessages" :key="index" :chat="item" />
  </div>
</template>