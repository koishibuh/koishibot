<script setup lang="ts">
import {onMounted} from 'vue';
import {useEventFeedStore} from './event-feed.store';

const store = useEventFeedStore();

onMounted(async () => {
  try {
    if (store.streamEvents.length === 0) {
      await store.getRecentEvents();
    }
  } catch (error) {
    console.log(error);
  }
});
</script>

<template>
  <div class="p-2 flex flex-col-reverse">
    <div  v-if="store.streamEvents?.length != 0" v-for="(item, index) in store.streamEvents" :key="index" class="flex mb-2">
      <div class="bg-accent-one p-2 rounded-l-lg">{{ item.timestamp }}</div>
      <div class="bg-foreground text-black p-2 w-full rounded-r-lg">{{ item.message }}</div>
    </div>
  </div>
</template>