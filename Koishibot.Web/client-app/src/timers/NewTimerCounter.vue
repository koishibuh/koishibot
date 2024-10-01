<script setup lang="ts">
import {ref, computed, defineProps, onMounted, watch} from 'vue';

const emits = defineEmits<{ (e: 'updateTimer'): void }>();
const props = defineProps<{ duration: string }>();

onMounted(() => {
  const [hours, minutes, seconds] = props.duration.split(":").map(Number);
  currentHour.value = hours;
  currentMinute.value = minutes;
  currentSecond.value = seconds;
  startTimer();
});

const currentHour = ref(0);
const currentMinute = ref(0);
const currentSecond = ref(0);
let intervalId: number = 1000;

const startTimer = function () {
  if (intervalId) clearInterval(intervalId);

  intervalId = setInterval(() => {
    if (currentSecond.value === 0) {
      emits('updateTimer');
      if (currentMinute.value === 0) {
        clearInterval(intervalId);
      } else {
        currentMinute.value--;
        currentSecond.value = 59;
        emits('updateTimer');
      }
    } else {
      currentSecond.value--;
      emits('updateTimer');
    }
  }, 1000);
};

const formattedMinutes = computed(() => {
  return currentMinute.value < 1 ? '00' : currentMinute.value.toString().padStart(2, '0');
});

const formattedSeconds = computed(() => {
  return currentSecond.value.toString().padStart(2, '0')
})

watch(() => props.duration, () => {
  startTimer();
});
</script>

<template>
  <div class="text-white font-bold text-7xl/[58px] drop-shadow-[0_1.2px_1.2px_rgba(0,0,0,0.8)]">
    {{ formattedMinutes }}:{{ formattedSeconds }}
  </div>
</template>