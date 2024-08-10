<script setup lang="ts">
import ToggleButton from '@/common/toggle-button.vue';
import { useServiceStatusStore } from '@/layout/service-status.store';
import { useTwitchStore } from './twitch.store';
import type { Component } from 'vue';

const props = defineProps<{
  settings: Component;
}>();

const emit = defineEmits<{
  settingClicked: [string];
}>();

const store = useServiceStatusStore();
const twitchstore = useTwitchStore();

const updateIRCStatus = async (status: boolean) => {
  await twitchstore.updateIrcStatus(status);
};

const updateEventSubStatus = async (status: boolean) => {
  await twitchstore.updateIrcStatus(status);
};
</script>

<template>
  <div class="flex">
    <ToggleButton
      button1Name="Enable IRC"
      button2Name="Disable IRC"
      @update-state="updateIRCStatus"
    />

    <ToggleButton
      button1Name="Enable TTV EventSub"
      button2Name="Disable TTV EventSub"
      @update-state="updateEventSubStatus"
    />
  </div>
</template>
