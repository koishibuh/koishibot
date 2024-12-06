<script setup lang="ts">
import {ref} from 'vue';
import {useAuthenticationStore} from '@/login/authentication.store';
import {useRouter} from 'vue-router';

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
    route.push({name: 'Home'});
  } catch (error) {
    console.log(error);
  }
}
</script>

<template>
  <div class="flex flex-col h-dvh bg-secondary">
    <div class="hidden fixed lg:block left-0 w-full fill-primary origin-center rotate-180 top-0 overflow-hidden z-10">
      <svg class="relative w-119 h-[250px]" data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
        <path d="M985.66,92.83C906.67,72,823.78,31,743.84,14.19c-82.26-17.34-168.06-16.33-250.45.39-57.84,11.73-114,31.07-172,41.86A600.21,600.21,0,0,1,0,27.35V120H1200V95.8C1132.19,118.92,1055.71,111.31,985.66,92.83Z" class="shape-fill"></path>
      </svg>
    </div>

    <div class="hidden fixed left-0 w-full lg:block fill-primary bottom-0 overflow-hidden z-10">
      <svg class="relative w-119 h-[250px]" data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
        <path d="M985.66,92.83C906.67,72,823.78,31,743.84,14.19c-82.26-17.34-168.06-16.33-250.45.39-57.84,11.73-114,31.07-172,41.86A600.21,600.21,0,0,1,0,27.35V120H1200V95.8C1132.19,118.92,1055.71,111.31,985.66,92.83Z" class="shape-fill"></path>
      </svg>
    </div>

    <div class="w-[400px] m-auto">
      <form @submit.prevent="login" class="flex flex-col p-2 gap-2 my-2 bg-background rounded border-2 border-background">
        <label for="username" class="text-foreground"> Username: </label>
        <input v-model="username" type="text" name="username" class="p-2"/>

        <label for="password" class="text-foreground"> Password: </label>
        <input v-model="password" type="password" name="password" class="p-2"/>

        <button type="submit" name="button" class="login-button">Login</button>
      </form>
    <p class="text-center">{{ formattedBuildTimestamp }}</p>
    </div>
  </div>
</template>