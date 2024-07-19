<script setup lang="ts">
import { ref, onMounted, watch, onUnmounted, computed } from 'vue';
import { useTimerStore } from '@/common/timer.store';
// 1 hour for pomdodoro
// 5 break and starting
// 2 minute

const store = useTimerStore();
const title = ref(store.currentOverlayTimer.title);
const minutes = ref(store.currentOverlayTimer.minutes);
const seconds = ref(store.currentOverlayTimer.seconds);
let intervalId: number = 1000;

const startTimer = function () {
  if (intervalId) clearInterval(intervalId);

  intervalId = setInterval(() => {
    if (seconds.value === 0) {
      if (minutes.value === 0) {
        clearInterval(intervalId);
      } else {
        minutes.value--;
        seconds.value = 59;
      }
    } else {
      seconds.value--;
    }
  }, 1000);
};

const formattedMinutes = computed(() => {
  return minutes.value < 1 ? '00' : minutes.value.toString().padStart(2, '0');
});

watch(
  () => store.currentOverlayTimer,
  (newTimer) => {
    title.value = newTimer.title;
    minutes.value = newTimer.minutes;
    seconds.value = newTimer.seconds;
    startTimer(); // start the timer when the store updates
  },
  { deep: true } // watch for nested property changes
);

onUnmounted(() => {
  clearInterval(intervalId);
});
</script>

<template>
  <div class="w-1/5 flex">
    <div class="w-1/2 flex">
      <p
        class="uppercase text-white font-bold text-4xl text-end pr-2 word leading-[2rem] drop-shadow-[0_1.2px_1.2px_rgba(0,0,0,0.8)]"
      >
        {{ title }}
      </p>
    </div>
    <div class="text-white font-bold text-7xl/[58px] drop-shadow-[0_1.2px_1.2px_rgba(0,0,0,0.8)]">
      {{ formattedMinutes }}:{{ seconds.toString().padStart(2, '0') }}
    </div>
  </div>
</template>
