<script setup lang="ts">
import {computed, ref, type Ref, watch} from "vue";
import type {IGoal} from "@/event-feed/event-feed.store.ts";

const props = defineProps<{
  goal: Ref<IGoal>,
  addDecimal: boolean
}>();

const waveWidth = 40;

const isProgressUpdating = ref(false);
const showPulse = ref(false);

const progress = computed((): number => {
  if (props.goal.value.currentAmount > 0) {
    const newProgress = Math.floor((props.goal.value.currentAmount / props.goal.value.goalAmount) * 100);
    if (newProgress !== progress.value) { 
      isProgressUpdating.value = true;
      setTimeout(() => {
        isProgressUpdating.value = false;
      }, 2000);  // Flash duration
    }
    return newProgress;
  } else {
    return 0;
  }
});

watch(progress, (newValue) => {
  if (newValue >= 100) {
    showPulse.value = true;

    setTimeout(() => {
      showPulse.value = false;
    }, 3000);
  }
});

const formatWithDecimal = (num: number): string => {
  if (num === 0) return '0.00';

  const str = Math.floor(num).toString();

  if (str.length <= 2) {
  
    return `.${str.padStart(2, '0')}`;
  }
 
  return str.slice(0, -2) + '.' + str.slice(-2);
};

const goalAmount = computed(() => {
  return props.addDecimal
      ? formatWithDecimal(props.goal.value.goalAmount)
      : props.goal.value.goalAmount.toString();
});

const currentAmount = computed(() => {
  return props.addDecimal
      ? formatWithDecimal(props.goal.value.currentAmount)
      : props.goal.value.currentAmount.toString();
});
</script>

<template>
  <h1>{{props.goal.value.title}}</h1>
  <div class="w-full max-w-xl h-6 bg-[#988fac] rounded overflow-hidden relative">
    <div class="h-full  relative">
      <div
          class="h-full  relative progress"
          :class="[progress >= 100 ? 'bg-[#f28eaa]' : 'bg-[#2874b8]',
          isProgressUpdating ? 'bg-[#f28eaa]' : 'bg-[#2874b8]']"
          :style="{ width: `${progress}%` }"
      >
        <svg
            class="absolute top-0 right-[-43px] h-[60px]"
            :style="{ width: waveWidth * 2 + 'px' }"
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 82 24"
            preserveAspectRatio="none"
        >
          <path
              :fill="isProgressUpdating ? '#f28eaa' : '#2874b8'"
              d="M0 12 Q10 0 20 12 T40 12 T60 12 T80 12 V24 H0 Z"
              class="wave-path"
          />
        </svg>
      </div>
    </div>
    
    <div
        class="absolute left-1/2 top-1/2 transform -translate-x-1/2 -translate-y-1/2 text-white font-semibold pointer-events-none"
    >
      {{ currentAmount }} / {{ goalAmount }}
    </div>
        <div v-if="showPulse" class="pulse " ref="pulseEffect">🌟</div>
  </div>
</template>


<style scoped>
.wave-path {
  transform-origin: center;
  animation: wave-animation 6s linear infinite;
  transform-box: fill-box;
  transform: rotate(90deg);
}

@keyframes wave-animation {
  0% {
    transform: rotate(90deg) translateX(0);
  }
  100% {
    transform: rotate(90deg) translateX(-40px); /* half the wave width */
  }
}

/* Prevent the wave SVG from blocking pointer events */
svg {
  pointer-events: none;
  transform-origin: center;
}

.progress {
  height: 100%;
  border-radius: 10px;
  transition: width 2s ease;
  position: relative; /* add this */
}

.pulse {
  position: absolute;
  top: 2px;
  right: 1px;
  width: 20px;
  height: 20px;
  border-radius: 50%;
  animation: pulse-animation 1s 3 ease-in-out;
}

@keyframes pulse-animation {
  0%,
  100% {
    transform: scale(1);
    opacity: 1;
  }

  50% {
    transform: scale(1.5); /* Scale up */
    opacity: 0.5; /* Fade out */
  }
}
</style>