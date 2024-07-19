<script setup lang="ts">
import { onMounted } from 'vue';
import { useNotificationStore } from '@/common/notification.store';

const store = useNotificationStore();

onMounted(async () => {
  try {
    await store.GetStatus();
  } catch (error) {
    console.log(error);
  }
});

function statusColor(status: Boolean): string {
  return status ? 'bg-g' : 'bg-red-500';
}
</script>

<template>
  <div
    v-for="item in store.serviceStatuses"
    :key="item.name"
    class="tooltip h-3 w-3 rounded-full"
    :class="statusColor(item.status)"
    :alt="item.name"
  >
    <span class="tooltiptext">{{ item.name }}</span>
  </div>
</template>

<!-- <div class="tooltip bg-g h-3 w-3 rounded-full" alt="Stream Elements Status">
	<span class="tooltiptext">Stream Elements Status</span>
</div>
<div class="tooltip bg-g h-3 w-3 rounded-full" alt="Streamer Client Status">
	<span class="tooltiptext">Streamer Client Status</span>
</div>
<div class="tooltip bg-g h-3 w-3 rounded-full" alt="Bot Client Status">
	<span class="tooltiptext">Bot Client Status</span>
</div>
<div class="tooltip bg-g h-3 w-3 rounded-full" alt="Event Sub Status">
	<span class="tooltiptext">Event Sub Status</span>
</div>
<div class="tooltip bg-g h-3 w-3 rounded-full" alt="Chat Processor Status">
	<span class="tooltiptext">Chat Processor Status</span>
</div> -->
