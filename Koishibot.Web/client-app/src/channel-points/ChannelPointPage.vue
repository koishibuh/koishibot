<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useChannelPointStore } from './channel-points-store';
import http from '@/api/http';
import CreateReward from './CreateReward.vue';

const store = useChannelPointStore();

onMounted(() => {
  store.getChannelPointRewards();
});
</script>

<template>
  <div class="p-2 mb-2 mx-auto max-w-xl">
    <a href="https://dashboard.twitch.tv/viewer-rewards/channel-points/rewards" target="_blank"
      >Twitch Rewards</a
    >
    <h1>Channel Points</h1>
    <table class="table-fixed w-full border rounded mb-4">
      <thead>
        <tr>
          <th>Title</th>
          <!-- <th>Description</th> -->
          <th>Cost</th>
          <!-- <th>Background Color</th> -->
          <th>Enabled</th>
          <!-- <th>UserInput</th> -->
          <!-- <th>MaxPerStream</th>
          <th>MaxPerUser</th>
          <th>Global Cooldown</th> -->
          <th>Paused</th>
          <!--   <th>Skip Redemption Queue</th> -->
        </tr>
      </thead>
      <tbody v-if="store.channelPointRewards">
        <tr v-for="reward in store.channelPointRewards" :key="reward.title">
          <td>{{ reward.title }}</td>
          <td>{{ reward.cost }} points</td>
          <td>{{ reward.isEnabled }}</td>
          <td>{{ reward.isPaused }}</td>
        </tr>
      </tbody>
    </table>

    <h1>Create Channel Points</h1>
    <CreateReward />
  </div>
</template>
