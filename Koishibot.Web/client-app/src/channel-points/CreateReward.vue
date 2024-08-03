<script setup lang="ts">
import { ref, watch } from 'vue';
import { useChannelPointStore } from './channel-points-store';
import { type IChannelRewardRequest } from './models/reward-request.interface';

const store = useChannelPointStore();

const title = ref('');
const cost = ref(0);
const isEnabled = ref();
const userInputRequired = ref();
const prompt = ref('');
const rewardBackgroundColor = ref('');
const isMaxPerStream = ref();
const maxPerStream = ref(0);
const isMaxPerUserPerStream = ref();
const maxPerUserPerStream = ref(0);
const rewardIsGlobalCooldown = ref();
const rewardCooldown = ref();
const skipQueue = ref();

const createReward = async () => {
  const reward: IChannelRewardRequest = {
    title: title.value,
    cost: cost.value,
    isEnabled: isEnabled.value,
    isUserInputRequired: userInputRequired.value,
    prompt: prompt.value,
    backgroundColor: rewardBackgroundColor.value,
    isMaxPerStreamEnabled: isMaxPerStream.value,
    maxPerStream: maxPerStream.value,
    isMaxPerUserPerStreamEnabled: isMaxPerUserPerStream.value,
    maxPerUserPerStream: maxPerUserPerStream.value,
    isGlobalCooldownEnabled: rewardIsGlobalCooldown.value,
    globalCooldownSeconds: rewardCooldown.value,
    shouldRedemptionsSkipRequestQueue: skipQueue.value
  };

  await store.createChannelPointReward(reward);
  title.value = '';
  cost.value = 0;
  isEnabled.value = null;
  userInputRequired.value = null;
  prompt.value = '';
  rewardBackgroundColor.value = '';
  isMaxPerStream.value = null;
  maxPerStream.value = 0;
  isMaxPerUserPerStream.value = null;
  maxPerUserPerStream.value = 0;
  rewardIsGlobalCooldown.value = null;
  rewardCooldown.value = 0;
  skipQueue.value = null;
};
</script>

<template>
  <div class="border-2 p-2 border-gray-500 rounded">
    <form @submit.prevent="createReward()" class="flex flex-col gap-2 my-4">
      <div class="flex flex-col">
        <label for="title">Title:</label>
        <input type="text" v-model="title" id="title" class="text-black" />
      </div>

      <div class="flex flex-col">
        <label for="cost">Cost:</label>
        <input type="text" v-model="cost" id="cost" class="text-black" />
      </div>

      <div class="flex flex-wrap justify-around">
        <div class="flex gap-2">
          <label for="enabled">IsEnabled:</label>
          <input type="checkbox" v-model="isEnabled" id="enabled" class="text-black" />
        </div>

        <div class="flex gap-2">
          <label for="inputRequired">User Input Required:</label>
          <input
            type="checkbox"
            v-model="userInputRequired"
            id="inputRequired"
            class="text-black"
          />
        </div>

        <div class="flex gap-2">
          <label for="skipQueue">SkipRequestQueue:</label>
          <input type="checkbox" v-model="skipQueue" id="skipQueue" class="text-black" />
        </div>
      </div>

      <label for="prompt">Prompt:</label>
      <input type="text" v-model="prompt" id="prompt" class="text-black" />

      <label for="backgroundColor">BackgroundColor:</label>
      <input type="text" v-model="rewardBackgroundColor" id="backgroundColor" class="text-black" />

      <div class="flex justify-around">
        <div class="flex gap-2">
          <label for="isMaxPerStream">Enabled:</label>
          <input type="checkbox" v-model="isMaxPerStream" id="isMaxPerStream" class="text-black" />
        </div>

        <div class="flex gap-2">
          <label for="maxPerStream">MaxPerStream:</label>
          <input type="text" v-model="maxPerStream" id="maxPerStream" class="text-black" />
        </div>
      </div>

      <div class="flex justify-around">
        <div class="flex gap-2">
          <label for="isMaxPerUserPerStream">Enabled:</label>
          <input
            type="checkbox"
            v-model="isMaxPerUserPerStream"
            id="isMaxPerUserPerStream"
            class="text-black"
          />
        </div>

        <div class="flex gap-2">
          <label for="maxPerUserPerStream">MaxPerUserPerStream:</label>
          <input
            type="text"
            v-model="maxPerUserPerStream"
            id="maxPerUserPerStream"
            class="text-black"
          />
        </div>
      </div>

      <div class="flex justify-around">
        <div class="flex gap-2">
          <label for="isGlobalCooldown">Enabled:</label>
          <input
            type="checkbox"
            v-model="rewardIsGlobalCooldown"
            id="isGlobalCooldown"
            class="text-black"
          />
        </div>

        <div class="flex gap-2">
          <label for="globalCooldown">GlobalCooldownSeconds:</label>
          <input type="text" v-model="rewardCooldown" id="globalCooldown" class="text-black" />
        </div>
      </div>

      <button class="primary-button">Send</button>
    </form>
  </div>
</template>
