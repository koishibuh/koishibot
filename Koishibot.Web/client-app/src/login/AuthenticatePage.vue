<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthenticationStore } from './authentication.store';

const router = useRouter();
const store = useAuthenticationStore();
const message = ref<any>();

onMounted(async () => {
  if (router.currentRoute.value.query.code) {
    try {
      const token = await store.getTwitchOAuthToken(router.currentRoute.value.query.code as string);
      if (token) {
        console.log('token received', token);
        message.value = token;
        router.push({ name: 'Home' });
      }
    } catch (error) {
      console.log(error);
    }
  }
});
</script>

<template>
  <h1>Authenticating</h1>
  {{ message }}
</template>
