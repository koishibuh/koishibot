<script setup lang="ts">
import { ref, watch } from 'vue';
import { useAdTimerStore } from './adtimer.store';

const store = useAdTimerStore();
const progress = ref(100);
const duration = ref<number>(180000); // 3 minutes in milliseconds
const enable = ref<Boolean>(true);

async function startTimer() {
  progress.value = 100;
  enable.value = true;

  const intervalId = setInterval(() => {
    progress.value -= 100 / (duration.value / 1000);

    if (progress.value <= 0) {
      clearInterval(intervalId);
      sleep(3000).then(() => (enable.value = false));
    }
  }, 1000); // decrement every second
}

const sleep = (ms: number) => {
  return new Promise((resolve, reject) => setTimeout(resolve, ms));
};

watch(() => store.adTimer, (newValue, oldValue) => {
  if (newValue) {
    startTimer();
  }
})
</script>

<template>
  <div class="h-[200px] flex items-center">
    <transition name="slide-up">
      <div v-if="enable" class="w-full">
        <div class="text-4xl font-bold">Ad Timer</div>
        <div class="progress-bar top-10">
          <div class="progress" :style="{ width: `${progress}%` }"></div>
        </div>
      </div>
    </transition>
  </div>
</template>

<style scoped>
.slide-up-enter-active,
.slide-up-leave-active {
  transition: all 0.1s ease-out;
}

.slide-up-enter-from {
  opacity: 0;
  transform: translateY(30px);
}

.slide-up-leave-to {
  opacity: 0;
  transform: translateY(-30px);
}

.progress-bar {
  width: 50%;
  height: 25px;
  background-color: #ccc;
  border-radius: 10px;
}

.progress {
  height: 100%;
  background-color: purple;
  border-radius: 10px;
  transition: width 2s ease;
}
</style>