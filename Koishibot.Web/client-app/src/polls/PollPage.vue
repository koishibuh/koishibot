<script setup lang="ts">
import {ref, watch, computed} from 'vue';
import {usePollStore} from './poll-store';
import LabelField from "@/common/label-field/LabelField.vue";
import {storeToRefs} from "pinia";

const store = usePollStore();
const { pollVotes } = storeToRefs(store);

const minutes = ref(0);
const seconds = ref(0);
let intervalId: number = 1000;

function convertSecondsToMinutesAndSeconds(time: string) {
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

const isLoading = ref<boolean>();

const submitPoll = (async () => {
  isLoading.value = true;
  await store.submitPoll();
  isLoading.value = false;
})

const cancelPoll = (async () => {
  isLoading.value = true;
  await store.cancelPoll();
  isLoading.value = false;
})

watch(
    () => store.pollDuration,
    (newTimer) => {
      convertSecondsToMinutesAndSeconds(newTimer);
      startTimer(); // start the timer when the store updates
    },
    {deep: true} // watch for nested property changes
);
</script>

<template>
  <form v-if="store.displayForm" @submit.prevent="submitPoll" class="flex flex-col gap-2 my-4">
    <LabelField :text-limit="60" label="Poll Title" label-id="pollTitle" :text="store.pendingPoll.title"/>

    <div class="flex flex-wrap justify-evenly gap-2">
      <LabelField :text-limit="25" label="Choice 1" label-id="choiceOne"  :text="store.pendingPoll.choices[0]"/>
      <LabelField :text-limit="25" label="Choice 2" label-id="choiceTwo" :text="store.pendingPoll.choices[1]"/>
      <LabelField :text-limit="25" label="Choice 3" label-id="choiceThree" :text="store.pendingPoll.choices[2]"/>
      <LabelField :text-limit="25" label="Choice 4" label-id="choiceFour" :text="store.pendingPoll.choices[3]"/>
      <LabelField :text-limit="25" label="Choice 5" label-id="choiceFive" :text="store.pendingPoll.choices[4]"/>
    </div>

    <label for="duration">Duration</label>
    <div id="toggle" class="flex justify-evenly gap-2">
      <input type="radio" id="seconds30" v-model="store.pendingPoll.duration" value="30">
      <label for="seconds30">30 seconds</label>
      <input type="radio" id="minute3" v-model="store.pendingPoll.duration" value="180">
      <label for="minute3">3 Minutes</label>
      <input type="radio" id="minute5" v-model="store.pendingPoll.duration" value="300" checked>
      <label for="minute5">5 Minutes</label>
      <input type="radio" id="minute10" v-model="store.pendingPoll.duration" value="600">
      <label for="minute10">10 Minutes</label>
      <input type="radio" id="minute15" v-model="store.pendingPoll.duration" value="900">
      <label for="minute15">15 Minutes</label>
      <input type="radio" id="minute20" v-model="store.pendingPoll.duration" value="1200">
      <label for="minute20">20 Minutes</label>
      <input type="radio" id="minute30" v-model="store.pendingPoll.duration" value="1800">
      <label for="minute30">30 Minutes</label>
    </div>

    <button class="primary-button" :disabled="isLoading">Send</button>
  </form>
  <div v-else>
    <div v-if="store.pollVotes" class="mt-2 border-2 rounded p-2 mb-2">

      <p>Title: {{ store.pollTitle }}</p>
      <div v-for="(item, key) in pollVotes" :key="key">
        <p>{{ item.choice }} - {{ item.voteCount }}</p>
      </div>

      {{ formattedMinutes }}:{{ seconds }}

    </div>
    <button @click="cancelPoll" class="primary-button" :disabled="isLoading">Cancel Poll</button>
  </div>
      Winner: {{store.winningChoice}}
</template>

<style scoped>

#toggle input {
  width: 0;
  height: 0;
  position: absolute;
  left: -9999px;
}

#toggle input + label {
  @apply rounded bg-gray-500 p-2 font-bold w-full text-center;
  position: relative;
  display: inline-block;
  transition: border-color 0.15s ease-out, color 0.25s ease-out, background-color 0.15s ease-out, box-shadow 0.15s ease-out;
}

#toggle input:checked + label {
  @apply bg-accent-two z-10;
}

#toggle input:focus + label {
  outline: dotted 1px #CCC;
}

#toggle input:hover + label {
  @apply bg-primary;
}
</style>