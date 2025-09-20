<script setup lang="ts">
import {computed, onMounted, ref} from "vue";
import {useGoalStore} from "@/goals/goal.store.ts";

const store = useGoalStore();

onMounted(async () => {
  try {
    {
      await store.getBitGoal();
    }
  } catch (error) {
    console.log(error);
  }
})

const text = ref<string>('!Goal');
const waveWidth = 40;

// const increase = (number: number) => {
//   progress.value = progress.value + number;
// }

const progress = computed(() => {
  if (store.bitGoal.currentAmount > 0)
  {
    return Math.floor((store.bitGoal.currentAmount / store.bitGoal.goalAmount) * 100);
  } else {
    return 0;
  }
})

</script>

<template>

<!--  <button @click="increase(5)">CLICK</button>-->

<div>Current Amount: {{store.bitGoal.currentAmount}}</div>
<div>Goal: {{store.bitGoal.goalAmount}}</div>
  
  <div class="w-full max-w-xl h-6 bg-gray-300 rounded overflow-hidden relative">
    <div
        class="h-full  relative"

    >
      <div
          class="h-full bg-blue-500 relative progress"
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
              fill="#3b82f6"
              d="M0 12 Q10 0 20 12 T40 12 T60 12 T80 12 V24 H0 Z"
              class="wave-path"
          />
        </svg>
      </div>
    </div>

    <!-- Centered floating text -->
    <div
        class="absolute left-1/2 top-1/2 transform -translate-x-1/2 -translate-y-1/2 text-white font-semibold pointer-events-none"
    >
      {{ store.bitGoal.currentAmount }} / {{ store.bitGoal.goalAmount }}
    </div>
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