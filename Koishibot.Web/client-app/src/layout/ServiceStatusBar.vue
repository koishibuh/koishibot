<script setup lang="ts">
import {onMounted} from 'vue';
import { useNotificationStore } from '@/common/notifications/notification.store';

const store = useNotificationStore();

onMounted(async () => {
  try {
    await store.getStatus();
  } catch (error) {
    console.log(error);
  }
});


const statusColor = (status: string): string => {
  switch (status) {
    case 'Online':
      return 'bg-g';
    case 'Loading':
      return 'bg-yellow-400';
    case 'Offline':
      return 'bg-red-500';
    default:
      return ' bg-gray-600';
  }
};
</script>

<template>
  <div v-if="store.serviceStatuses?.length != 0"
    v-for="item in store.serviceStatuses"
    :key="item.name"
    class="tooltip h-3 w-3 rounded-full"
    :class="statusColor(item.status)"
    :alt="item.name"
  >
    <span class="tooltiptext">{{ item.name }}</span>
  </div>
</template>