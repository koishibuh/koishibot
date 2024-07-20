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
  <toggleButton button1Name="Enable OBS" button2Name="Disable OBS" @enableItem="obs" />

  <div class="h-[200px] border-2 border-white rounded">
    <div v-if="store.logMessages.length > 0">
      <div v-for="(message, index) in store.logMessages" :key="index">
        <div v-if="message.level === 'i'">
          <p>🟢 {{ message.timestamp }} | {{ message.message }}</p>
        </div>
        <div v-else-if="message.level === 'w'">
          <p>🟡 {{ message.timestamp }} | {{ message.message }}</p>
        </div>
        <div v-else>
          <p>🔴 {{ message.timestamp }} | {{ message.timestamp }}</p>
        </div>
      </div>
    </div>
  </div>
</template>
