<script setup lang="ts">
import { ref, watch } from 'vue';
import { useAdTimerStore } from './adtimer.store';

const store = useAdTimerStore();
const progress = ref(100);
const enable = ref<Boolean>(false);

async function startTimer() {
  progress.value = 100;
  enable.value = true;

  const intervalId = setInterval(() => {
    progress.value -= 100 / (store.adTimer.adLength / 1000);

    if (progress.value <= 0) {
      clearInterval(intervalId);
      sleep(2000).then(() => (enable.value = false));
    }
  }, 1000); // decrement every second
}

const sleep = (ms: number) => {
  return new Promise((resolve, reject) => setTimeout(resolve, ms));
};

watch(() => store.adTimer, (newValue) => {
  if (newValue) {
    // duration.value = newValue.adLength;
    startTimer();
  }
})
</script>

<template>
  <div class="h-[200px] flex items-center">
    <transition name="slide-up">
      <div v-if="enable" class="w-full">
        <div class="progress-bar top-10">
          <div class="progress" :style="{ width: `${progress}%` }"><div class="icon">🐌</div></div>
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
  transform: translateY(-30px);
}

.slide-up-leave-to {
  opacity: 0;
  transform: translateY(30px);
}

.progress-bar {
  width: 50%;
  height: 25px;
  background-color: #6a45c0;
  border-radius: 10px;
  position: relative; /* add this */
}

.progress {
  height: 100%;
  background-color:  #ccc;
  border-radius: 10px;
  transition: width 2s ease;
  position: relative; /* add this */
}

.icon {
  position: absolute;
  right: -50px;
  top: -20%;
  transform: translateY(-50%);
  font-size: 50px;
}
</style>