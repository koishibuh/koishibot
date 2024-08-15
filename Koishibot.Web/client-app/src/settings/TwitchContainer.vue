<script setup lang="ts">
import ToggleButton from '@/common/toggle-button.vue';
import { useServiceStatusStore } from '@/layout/service-status.store';
import { useTwitchStore } from './twitch.store';
import type { Component } from 'vue';
import { ref } from 'vue';

const props = defineProps<{
  ircStatus: boolean;
  eventSubStatus: boolean;
}>();

const emit = defineEmits<{
  settingClicked: [string];
}>();

const store = useServiceStatusStore();
const twitchStore = useTwitchStore();

const updateIRCStatus = async (status: boolean) => {
  await twitchStore.updateIrcStatus(status);
};

const updateEventSubStatus = async (status: boolean) => {
  await twitchStore.updateEventSubStatus(status);
};

const connectToTwitch = async () => {
  const authUrl = await twitchStore.connectToTwitch();
  window.open(authUrl, '_self');
};

const reconnectTwitchServices = async () => {
  await twitchStore.reconnectTwitchServices();
}
</script>

<template>
  <div class="flex flex-col gap-2">
    <!--  <div>
      <label for="refreshtoken">Token</label>
      <input type="password" v-model="refreshtoken" id="refreshtoken" class="text-black" />
      <button @click="connectBot()" class="w-1/2 primary-button">Connect (Refresh)</button>
    </div> -->
    <button @click="connectToTwitch" class="w-1/2 primary-button">
      Connect to Twitch
    </button>
    <ToggleButton
      button1Name="Enable IRC"
      button2Name="Disable IRC"
      :state="ircStatus"
      @update-state="updateIRCStatus"
    />

    <ToggleButton
      button1Name="Enable TTV EventSub"
      button2Name="Disable TTV EventSub"
      :state="eventSubStatus"
      @update-state="updateEventSubStatus"
    />

    <button class="primary-button" @click="reconnectTwitchServices">Reconnect Stream Services</button>
  </div>
</template>