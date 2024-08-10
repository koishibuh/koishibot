<script setup lang="ts">
import { ref, onMounted, watch, computed } from 'vue';
import { useSettingsStore } from '@/settings/settings.store';
import ObsContainer from '@/settings/ObsContainer.vue';
import { useObsStore } from './obs.store';
import ModalContainer from '@/common/modal/ModalContainer.vue';
import ScopesContainer from './ScopesContainer.vue';
import TwitchContainer from './TwitchContainer.vue';
import { useServiceStatusStore } from '@/layout/service-status.store';

const store = useSettingsStore();
const obsStore = useObsStore();
const statusStore = useServiceStatusStore();
const message = ref(store.message);

const obs = async (e: boolean) => {
  if (e) {
    await store.UpdateObsConnection(true);
    message.value = 'Enabled';
  } else {
    message.value = 'Disabled';
    await store.UpdateObsConnection(false);
  }
};

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

  <h1>Twitch</h1>
  <div class="border-2 p-2 border-gray-500 rounded">
    <TwitchContainer />
  </div>

  <h1>OBS</h1>
  <div class="border-2 p-2 border-gray-500 rounded">
    <ObsContainer :settings="obsStore.settings" @setting-clicked="showModalContent" />
  </div>

  <h1>Scopes</h1>
  <div class="border-2 p-2 border-gray-500 rounded">
    <ScopesContainer @setting-clicked="showModalContent" />
  </div>
</template>
