<script setup lang="ts">
import { useEventFeedStore } from '@/event-feed/event-feed.store';

const store = useEventFeedStore();
</script>

<template>
  <div class="bg-accent-one flex gap-2 p-1 m-2 rounded">
    <!--  <div class="font-bold">Recent Event</div> -->
    <div class="bg-accent-one p-2 rounded-l-lg">⭐</div>
    <div class="bg-accent-one p-2 w-1/2">{{ store.recentEvent?.timestamp }}</div>
    <div class="text-black p-2 w-full rounded-r-lg">
      {{ store.recentEvent?.message }}
    </div>
  </div>
</template>
