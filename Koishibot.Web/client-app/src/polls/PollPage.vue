<script setup lang="ts">
import { ref, watch, computed } from 'vue';
import { usePollStore } from './poll-store';
import http from '@/api/http';

/* const poll = ref<IPendingPoll>(); */
const store = usePollStore();
const currentPollChoices = ref(store.currentPoll?.choices);

const pollTitle = ref('');
const choiceOne = ref('');
const choiceTwo = ref('');
const choiceThree = ref('');
const choiceFour = ref('');
const choiceFive = ref('');
const duration = ref('');

async function submitPoll() {
  const choices = [
    choiceOne.value,
    choiceTwo.value,
    choiceThree.value,
    choiceFour.value,
    choiceFive.value
  ];

  await http.post('/api/polls', {
    Title: pollTitle.value,
    Choices: choices.filter((x) => x),
    Duration: parseInt(duration.value)
  });
}

async function cancelPoll() {
  await http.patch('/api/polls');
}

const minutes = ref(0);
const seconds = ref(0);
let intervalId: number = 1000;

function convertSecondsToMinutesAndSeconds(time: string) {
  console.log('converting');
  console.log(duration);
  const [hours1, minutes1, seconds1] = time.split(':').map(Number);
  const totalSeconds = hours1 * 3600 + minutes1 * 60 + seconds1;
  const minutesFromTotalSeconds = Math.floor(totalSeconds / 60);
  const remainingSeconds = totalSeconds % 60;
  minutes.value = minutesFromTotalSeconds;
  seconds.value = remainingSeconds;
}

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
  () => store.currentPollDuration,
  (newTimer) => {
    console.log('watching');
    convertSecondsToMinutesAndSeconds(newTimer);
    startTimer(); // start the timer when the store updates
  },
  { deep: true } // watch for nested property changes
);
</script>

<template>
  <div class="p-2">
    <p class="text-xl">Text</p>
    <p class="text-xl">Text</p>
    <p class="text-xl">Text</p>
    <p class="text-xl">Text</p>
  </div>

  <form @submit.prevent="submitPoll()" class="flex flex-col gap-2 my-4">
    <label for="polltitle">Poll Title: {{ pollTitle }}</label>
    <input type="text" v-model="pollTitle" id="pollTitle" class="text-black" />
    <label for="choiceone">Choice 1: {{ choiceOne }}</label>
    <input type="text" v-model="choiceOne" id="choiceone" class="text-black" />
    <label for="choicetwo">Choice 2: {{ choiceTwo }}</label>
    <input type="text" v-model="choiceTwo" id="choicetwo" class="text-black" />
    <label for="choicethree">Choice 3:</label>
    <input type="text" v-model="choiceThree" id="choicethree" class="text-black" />
    <label for="choicefour">Choice 4:</label>
    <input type="text" v-model="choiceFour" id="choicefour" class="text-black" />
    <label for="choicefive">Choice 5:</label>
    <input type="text" v-model="choiceFive" id="choicefive" class="text-black" />
    <label for="duration">Duration</label>
    <input type="text" v-model="duration" id="duration" class="text-black" />
    <button class="primary-class">Send</button>
  </form>

  <button @click="cancelPoll()">Cancel</button>

  <div class="mt-2 border-2 rounded p-2">
    Current Poll
    {{ currentPollChoices?.title }}

    <div v-for="(value, key) in currentPollChoices" :key="key" class="">
      {{ key }}
      {{ value }}
    </div>

    {{ formattedMinutes }}
    {{ seconds }}
  </div>
</template>
