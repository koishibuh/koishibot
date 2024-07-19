<script setup lang="ts">
import { ref, onMounted, watch, computed } from 'vue';
import toggleButton from '@/common/toggle-button.vue';
import { useSettingsStore } from '@/settings/settings.store';
import { useNotificationStore } from '@/common/notification.store';

const store = useSettingsStore();
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

const obsEnabled = ref<boolean>(true);
</script>

<template>
  <p>{{ message }}</p>

  <toggleButton button1Name="Enable OBS" button2Name="Disable OBS" @enableItem="obs" />
</template>
