<script setup lang="ts">
import {usePollStore} from "@/polls/poll-store";
import TimerCounter from "@/timers/NewTimerCounter.vue";
import PollBox from "@/polls/PollBox.vue";
import {storeToRefs} from "pinia";

const store = usePollStore();
const { pollDuration, enableOverlay } = storeToRefs(store);
</script>

<template>
  <div class="bg-gray-500 p-2 rounded border" v-if="store.enableOverlay">
    <div class="flex justify-between">
      <div class="text-2xl text-bold p-2">{{ store.pollTitle }}</div>
      <TimerCounter :duration="pollDuration"/>
    </div>
    <ul
        v-for="(item, key) in store.pollVotes" :key="key"
        class="flex flex-col rounded bg-pink-300 mb-2 p-2"
    >
      <PollBox :pollChoice="item" :totalVotes="store.voteCountTotal"></PollBox>
    </ul>
  </div>
</template>