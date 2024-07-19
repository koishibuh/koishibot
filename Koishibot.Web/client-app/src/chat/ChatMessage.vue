<script setup lang="ts">
import { computed } from 'vue';
import { type IChatMessage } from './chat-message.interface';

const props = defineProps<{
  chat: IChatMessage;
}>();

const nameColor = computed(() => {
  const r = props.chat.color.substring(1, 3);
  const g = props.chat.color.substring(3, 5);
  const b = props.chat.color.substring(5, 7);

  const bgDelta = parseInt(r, 16) * 0.299 + parseInt(g, 16) * 0.587 + parseInt(b, 16) * 0.114;
  return bgDelta > 150 ? '#000' : '#FFF';
});
</script>

<template>
  <div class="mb-2 border border-black rounded-lg">
    <div class="flex gap-2 px-2 py-1 rounded-t-lg" :style="{ backgroundColor: chat.color }">
      <div>🐟</div>
      <p class="grow font-bold" :style="{ color: nameColor }">{{ chat.username }}</p>
      <div class="pe-1">•••</div>
    </div>
    <p class="bg-foreground p-2 rounded-b-lg">
      {{ chat.message }}
    </p>
  </div>
</template>
