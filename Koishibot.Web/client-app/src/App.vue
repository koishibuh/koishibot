<script setup lang="ts">
import { RouterView } from 'vue-router';
import { watch, onMounted, ref } from 'vue';
import { useAuthenticationStore } from '@/login/authentication.store';

const authenticationStore = useAuthenticationStore();
onMounted(() => {
  console.log('test');
  authenticationStore.getUserData();
});
</script>

<template>
  <RouterView />
</template>
