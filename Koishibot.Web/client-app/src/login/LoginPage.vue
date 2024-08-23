<script setup lang="ts">
import { ref } from 'vue';
import { useAuthenticationStore } from '@/login/authentication.store';
import { useRouter } from 'vue-router';

const store = useAuthenticationStore();
const route = useRouter();

const username = ref<string>('');
const password = ref<string>('');
const buildTimestamp = ref(__BUILD_TIMESTAMP__);

const formattedBuildTimestamp = ref(new Date(buildTimestamp.value).toLocaleString('en-GB', {
  year: 'numeric',
  month: '2-digit',
  day: '2-digit',
  hour: '2-digit',
  minute: '2-digit',
  hour12: false
}).replace(',', ''));

async function login() {
  try {
    await store.loginUser(username.value, password.value);
    route.push({ name: 'Home' });
  } catch (error) {
    console.log(error);
  }
}
</script>

<template>
  <div class="w-1/2 mx-auto">
    <form @submit.prevent="login" class="flex flex-col border-2 p-2 gap-2 rounded my-2">
      <label for="username"> Username: </label>
      <input v-model="username" type="text" name="username" class="p-2" />

      <label for="password"> Password: </label>
      <input v-model="password" type="password" name="password" class="p-2" />

      <button type="submit" name="button" class="login-button">Login</button>
    </form>
  </div>

  <p class="text-center">{{ formattedBuildTimestamp }}</p>
</template>