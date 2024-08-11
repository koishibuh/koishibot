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

const startTwitchServices = async () => {
  const authUrl = await twitchStore.startTwitchServices();
  window.open(authUrl, '_self');
};
</script>

<template>
  <div class="flex flex-col gap-2">
    <!--  <div>
      <label for="refreshtoken">Token</label>
      <input type="password" v-model="refreshtoken" id="refreshtoken" class="text-black" />
      <button @click="connectBot()" class="w-1/2 primary-button">Connect (Refresh)</button>
    </div> -->
    <button @click="startTwitchServices" class="w-1/2 primary-button">
      Connect to Twitch Services
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
  </div>
</template>
