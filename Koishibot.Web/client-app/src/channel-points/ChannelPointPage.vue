<script setup lang="ts">
import { ref } from 'vue';
import http from '@/api/http';
const textfield = ref('');

const rewardTitle = ref('');
const rewardCost = ref('');
const isEnabled = ref();
const userInputRequired = ref();
const rewardPrompt = ref('');
const rewardBackgroundColor = ref('');
const isMaxPerStream = ref();
const maxPerStream = ref('');
const isMaxPerUserPerStream = ref();
const maxPerUserPerStream = ref('');
const rewardIsGlobalCooldown = ref();
const rewardCooldown = ref('');
const skipQueue = ref();

async function createReward() {
  await http.post('/api/point-reward', {
    Title: rewardTitle.value,
    Cost: rewardCost.value,
    IsEnabled: isEnabled.value,
    IsUserInputRequired: userInputRequired.value,
    Prompt: rewardPrompt.value,
    BackgroundColor: rewardBackgroundColor.value,
    IsMaxPerStreamEnabled: isMaxPerStream.value,
    MaxPerStream: parseInt(maxPerStream.value),
    IsMaxPerUserPerStreamEnabled: isMaxPerUserPerStream.value,
    MaxPerUserPerStream: parseInt(maxPerUserPerStream.value),
    IsGlobalCooldownEnabled: rewardIsGlobalCooldown.value,
    GlobalCooldownSeconds: parseInt(rewardCooldown.value),
    ShouldRedemptionsSkipRequestQueue: skipQueue.value
  });
}
</script>

<template>
  <div class="p-2 mb-2 mx-auto max-w-xl">
    <h1>Channel Points</h1>
    <div class="border-2 p-2 border-gray-500 rounded">
      {{ textfield }}

      <form @submit.prevent="createReward()" class="flex flex-col gap-2 my-4">
        <div class="flex flex-col">
          <label for="title">Title:</label>
          <input type="text" v-model="rewardTitle" id="title" class="text-black" />
        </div>

        <div class="flex flex-col">
          <label for="cost">Cost:</label>
          <input type="text" v-model="rewardCost" id="cost" class="text-black" />
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
        <input type="text" v-model="rewardPrompt" id="prompt" class="text-black" />

        <label for="backgroundColor">BackgroundColor:</label>
        <input
          type="text"
          v-model="rewardBackgroundColor"
          id="backgroundColor"
          class="text-black"
        />

        <div class="flex justify-around">
          <div class="flex gap-2">
            <label for="isMaxPerStream">Enabled:</label>
            <input
              type="checkbox"
              v-model="isMaxPerStream"
              id="isMaxPerStream"
              class="text-black"
            />
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
  </div>
</template>
