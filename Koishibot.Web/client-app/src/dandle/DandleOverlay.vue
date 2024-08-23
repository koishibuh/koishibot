<script setup lang="ts">
import {ref, onMounted, computed, watch} from 'vue';
import {useDandleStore} from './dandle.store';
import TimerCounter from '@/timers/TimerCounter.vue';
import {type ITimer} from '@/timers/models/timer.interface';

import DandlePoll from './DandlePoll.vue';
import {useOverlayStatusStore} from '../common/overlay-status.store';

const dandleStore = useDandleStore();
const overlayStore = useOverlayStatusStore();

const updateTimer = (e: ITimer) => {
  dandleStore.dandleTimer.minutes = e.minutes;
  dandleStore.dandleTimer.seconds = e.seconds;

  if (e.minutes === 0 && e.seconds === 0) {
    dandleStore.displayTimer = false;
  }
};

watch(dandleStore.dandleTimer, () => {
  if (dandleStore.dandleTimer.minutes === 0 && dandleStore.dandleTimer.seconds === 0) {
    dandleStore.displayTimer = false;
  }
});
</script>

<template>
  <!--   <button class="primary-button" @click="addSuggestion">add suggestion</button>
  <button class="bg-b rounded border-2 text-xl" @click="updateTimer">BUTTON</button>
  <button class="bg-b rounded border-2 text-xl" @click="enableTimer">ENABLE BUTTON</button> -->
  <div class="flex">

    <div
        v-if="overlayStore.dandleOverlayStatus"
        class="w-1/3 h-[1080px] ml-5 flex flex-col items-center bg-gray-800/75"
    >
      <div class="h-1/6">
        <div v-if="dandleStore.enableSuggestions" class="w-full">
          <TransitionGroup name="slide" tag="ul" class="h-[190px]">
            <li
                v-for="item in dandleStore.suggestedWords"
                :key="item.username"
                class="flex text-4xl rounded bg-pink-300 w-[600px] mb-2 p-2"
            >
              <div class="w-3/4 pl-2">{{ item.username }}</div>
              <div class="w-1/4 bg-white pl-2 uppercase">{{ item.word }}</div>
            </li>
          </TransitionGroup>
        </div>

        <div v-if="dandleStore.enablePoll" class="w-full">
          <TransitionGroup name="slide" tag="ul" class="h-[190px]">
            <li
                v-for="item in dandleStore.dandleVotes"
                :key="item.choice"
                class="flex text-4xl rounded bg-pink-300 w-[600px] mb-2 p-2"
            >
              <DandlePoll :pollChoice="item" :totalVotes="dandleStore.voteCountTotal"></DandlePoll>
            </li>
          </TransitionGroup>
        </div>
      </div>
      <div class="h-[120px]">
        <div v-if="dandleStore.newTimer" class="stroke mb-2">
          <div>
            {{ dandleStore.dandleTimer.status }}
          </div>
          <div class="flex justify-center">
            <TimerCounter
                @updateTimer="updateTimer"
                :minutes="dandleStore.dandleTimer.minutes"
                :seconds="dandleStore.dandleTimer.seconds"
            ></TimerCounter>
          </div>
        </div>
      </div>
      <div class="grow">
        <div
            v-for="(word, wordIndex) in dandleStore.guessedWords"
            :key="wordIndex"
            class="w-fit flex flex-col justify-center gap-1 mb-2 px-5"
        >
          <div class="flex gap-2">
            <div
                v-for="(item, itemIndex) in word.letters"
                :key="itemIndex"
                class="dandle-letter"
                :style="{
                backgroundImage: `linear-gradient(${item.color})`
              }"
            >
              <div class="self-center">{{ item.letter }}</div>
            </div>
          </div>
        </div>
      </div>
      <div class="h-[250px] flex flex-col items-center">
        <div v-for="(item, itemIndex) in dandleStore.keyboard" :key="itemIndex" class="flex h-2/6">
          <div v-for="(column, columnIndex) in item.row" :key="columnIndex" class="px-1">
            <div
                class="dandle-keyboard"
                :style="{
                backgroundImage: `linear-gradient(${column.color})`
              }"
            >
              <div>{{ column.letter }}</div>
            </div>
          </div>
        </div>
      </div>
      <div class="bg-gray-600 text-white w-1/4 text-4xl text-center">
        <p>Score Board</p>
        <p class="text-xl">{{ dandleStore.message }}</p>
        <div v-for="(item, itemIndex) in dandleStore.scoreBoard" :key="itemIndex">
          <ul>
            <li>{{ item.username }} - {{ item.points }} || {{ item.bonusPoints }}</li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.progress {
  transition: width 2s ease;
}
</style>