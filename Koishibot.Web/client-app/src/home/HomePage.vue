<script setup lang="ts">
import { ref, onMounted, watch } from 'vue';
import http from '@/api/http';
import { useNotificationStore } from '@/common/notification.store';
import StreamInfo from '@/stream-info/StreamInfo.vue';

const store = useNotificationStore();
const oauthLink = ref('');
const connectResult = ref(false);
const test = ref();
const textfield = ref('');

// have this check to whatever is received from bot on load
const checked = ref<Boolean>();

async function connectBot() {
  store.notificationMessage = 'Connecting..';
  connectResult.value = await http.post('/api/twitch-auth', { token: refreshtoken.value });
}

async function getauthorization() {
  store.notificationMessage = 'Getting Authorization Link..';
  oauthLink.value = await http.get('/api/twitch-auth/url');
}

async function testFeature() {
  console.log('Testing 1 2 3');
  test.value = await http.get('/api/test');
}

const bannerTest = () => {
  store.addMessage('3');
};

const refreshtoken = ref<string>('');

watch(
  () => connectResult.value,
  () => {
    if (connectResult.value === true) {
      oauthLink.value = '';
    }
  }
);

watch(
  () => checked.value,
  async () => {
    await http.patch('/api/service-status', { ServiceName: 'Attendance', Status: checked.value });
  }
);
</script>

<template>
  {{ textfield }}

  <StreamInfo />

  <h1>Connect Bot</h1>
  <div class="border-2 p-2 border-gray-500 rounded h-24">
    <div class="flex gap-2 w-full mb-2">
      <label for="refreshtoken">Token</label>
      <input type="password" v-model="refreshtoken" id="refreshtoken" class="text-black" />
      <button @click="connectBot()" class="w-1/2 primary-button">Connect (Refresh)</button>
      <button @click="getauthorization()" class="w-1/2 primary-button">
        Connect (Authorization)
      </button>
    </div>

    <p class="text-center">{{ store.notificationMessage }}</p>
    <p class="text-center">
      <a v-show="oauthLink" target="_blank" :href="oauthLink">Authorization Link</a>
    </p>
  </div>

  <div class="p-2 mb-2 mx-auto max-w-xl">
    <button @click="testFeature()" class="w-full primary-button">Test Button</button>

    <input type="checkbox" id="checkbox" v-model="checked" />
    <label for="checkbox">Attendance: {{ checked }}</label>
  </div>

  <button @click="bannerTest" class="primary-button">Trigger Banner</button>
</template>
