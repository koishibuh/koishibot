<script setup lang="ts">
import { ref, onMounted, watch, computed } from 'vue';
import { useSettingsStore } from '@/settings/settings.store';
import ObsContainer from '@/settings/ObsContainer.vue';
import { useObsStore } from './obs.store';
import ModalContainer from '@/common/modal/ModalContainer.vue';
import ScopesContainer from './ScopesContainer.vue';
import TwitchContainer from './TwitchContainer.vue';
import { useServiceStatusStore } from '@/layout/service-status.store';
import { useTwitchStore } from './twitch.store';
import {useStreamElementsStore} from "@/settings/streamelements.store";
import StreamElementsContainer from '@/settings/StreamElementsContainer.vue';

const store = useSettingsStore();
const obsStore = useObsStore();
const statusStore = useServiceStatusStore();
const twitchStore = useTwitchStore();
const streamElementsStore = useStreamElementsStore();
const buildTimestamp = ref(__BUILD_TIMESTAMP__);

const formattedBuildTimestamp = ref(new Date(buildTimestamp.value).toLocaleString('en-GB', {
  year: 'numeric',
  month: '2-digit',
  day: '2-digit',
  hour: '2-digit',
  minute: '2-digit',
  hour12: false
}).replace(',', ''));

/* const obs = async (e: boolean) => {
  if (e) {
    await obsStore.UpdateObsConnection(true);
    message.value = 'Enabled';
  } else {
    message.value = 'Disabled';
    await store.UpdateObsConnection(false);
  }
}; */

const message1 = ref<string>('');
const modalContent = ref<string>('');
const showModal = ref<boolean>(false);

const showModalContent = (componentName: string) => {
  modalContent.value = componentName;
  message1.value = componentName;
  showModal.value = true;
};

const closeModal = () => {
  showModal.value = false;
  modalContent.value = '';
};
</script>

<template>
  <div v-if="showModal">
    <ModalContainer :content="modalContent" @modal-closed="closeModal" />
  </div>
  <p class="text-center">{{ formattedBuildTimestamp }}</p>

  <h1>Twitch</h1>
  <div class="border-2 p-2 border-gray-500 rounded">
    <TwitchContainer
      :irc-status="twitchStore.ircStatus.status"
      :event-sub-status="twitchStore.eventSubStatus.status"
      @setting-clicked="showModalContent"
    />
  </div>

  <h1>OBS</h1>
  <div class="border-2 p-2 border-gray-500 rounded">
    <ObsContainer :settings="obsStore.settings" @setting-clicked="showModalContent"/>
  </div>

  <h1>StreamElements</h1>
  <div class="border-2 p-2 border-gray-500 rounded">
    <StreamElementsContainer />
  </div>

  <h1>Scopes</h1>
  <div class="border-2 p-2 border-gray-500 rounded">
    <ScopesContainer @setting-clicked="showModalContent" />
  </div>
</template>