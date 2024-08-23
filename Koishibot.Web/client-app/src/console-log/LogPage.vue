<script setup lang="ts">
import { ref, onMounted, watch, computed } from 'vue';
import { useSettingsStore } from '@/settings/settings.store';
const store = useSettingsStore();
</script>

<template>
  <div class="h-[200px] border-2 border-white rounded overflow-scroll">
    <div v-if="store.logMessages.length >= 1">
      <div v-for="(message, index) in store.logMessages" :key="index">
        <div v-if="message.level === 'Info'">
          <p>🟢 {{ message.timestamp }} | {{ message.message }}</p>
        </div>
        <div v-else-if="message.level === 'Warning'">
          <p>🟡 {{ message.timestamp }} | {{ message.message }}</p>
        </div>
        <div v-else>
          <p>🔴 {{ message.timestamp }} | {{ message.message }}</p>
        </div>
      </div>
    </div>
  </div>
</template>