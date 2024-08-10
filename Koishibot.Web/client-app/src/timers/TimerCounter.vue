<script setup lang="ts">
import { ref, computed, defineProps, onMounted, watch } from 'vue';
import type { ITimer } from './models/timer.interface';

const props = defineProps<ITimer>();

const emits = defineEmits<{ (e: 'updateTimer', timer: ITimer): void }>();

const currentMinute = ref(0);
const currentSecond = ref(0);
let intervalId: number = 1000;

const startTimer = function () {
  if (intervalId) clearInterval(intervalId);

  intervalId = setInterval(() => {
    if (currentSecond.value === 0) {
      emits('updateTimer', { minutes: currentMinute.value, seconds: currentSecond.value });
      if (currentMinute.value === 0) {
        clearInterval(intervalId);
      } else {
        currentMinute.value--;
        currentSecond.value = 59;

        emits('updateTimer', { minutes: currentMinute.value, seconds: currentSecond.value });
      }
    } else {
      currentSecond.value--;
      emits('updateTimer', { minutes: currentMinute.value, seconds: currentSecond.value });
    }
  }, 1000);
};

const formattedMinutes = computed(() => {
  return currentMinute.value < 1 ? '00' : currentMinute.value.toString().padStart(2, '0');
});

watch(props, () => {
  currentMinute.value = props.minutes;
  currentSecond.value = props.seconds;
  startTimer();
});
</script>

<template>
  <div class="text-white font-bold text-7xl/[58px] drop-shadow-[0_1.2px_1.2px_rgba(0,0,0,0.8)]">
    {{ formattedMinutes }}:{{ seconds.toString().padStart(2, '0') }}
  </div>
</template>
