<script setup lang="ts">
import {ref, computed} from 'vue';
import {type IPollChoiceInfo} from '@/raids/models/raid-poll.interface';

const props = defineProps<{
  pollChoice: IPollChoiceInfo;
  totalVotes: number;
}>();

const progress = computed(() => {
  if (props.totalVotes === 0) {
    return 0;
  } else {
    return Math.floor((props.pollChoice.voteCount / props.totalVotes) * 100);
  }
});

const plural = computed(() => {
   return props.pollChoice.voteCount > 1 ? "Votes" : "Vote";
});
</script>

<template>
  <div class="flex justify-between text-xl">
    <div class="w-full">{{ pollChoice.choice }}</div>
    <div class="mx-2 w-1/12 text-end">{{ pollChoice.voteCount }} {{ plural }}</div>
  </div>
  <div class="w-full bg-[#787c7e] dandle-poll-bar h-[20px] rounded self-center">
    <div class="progress rounded bg-[#85c0f9] h-full" :style="{ width: `${progress}%` }"></div>
  </div>
</template>

<style>
/* .progress-bar {
  width: 50%;
  height: 25px;
  background-color: #ccc;
  border-radius: 10px;
}
 */
.progress {
  /*   height: 100%;
  background-color: purple;
  border-radius: 10px; */
  transition: width 2s ease;
}
</style>