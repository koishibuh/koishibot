<script setup lang="ts">
import { ref, onMounted, watch } from 'vue';
import http from '@/api/http';
import { useNotificationStore } from '@/common/notifications/notification.store';
import StreamInfo from '@/home/stream-info/StreamInfo.vue';

const store = useNotificationStore();
const test = ref();

// have this check to whatever is received from bot on load
const checked = ref<Boolean>();

async function testFeature() {
  console.log('Testing 1 2 3');
  test.value = await http.get('/api/test');
}

const bannerTest = () => {
  store.displayMessage('3');
};

watch(
  () => checked.value,
  async () => {
    await http.patch('/api/service-status', { ServiceName: 'Attendance', Status: checked.value });
  }
);
</script>

<template>
  <StreamInfo />

  <h1>Test Zone</h1>
  <div class="flex gap-2 border-2 p-2 border-gray-500 rounded h-24">
    <button @click="testFeature()" class="primary-button">Test Button</button>
    <button @click="bannerTest" class="primary-button">Trigger Banner</button>
  </div>

  <div class="p-2 mb-2 mx-auto max-w-xl">
    <input type="checkbox" id="checkbox" v-model="checked" />
    <label for="checkbox">Attendance: {{ checked }}</label>
  </div>
</template>
