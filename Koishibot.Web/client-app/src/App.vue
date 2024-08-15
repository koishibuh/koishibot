<script setup lang="ts">
import { RouterView } from 'vue-router';
import { watch, onMounted, ref } from 'vue';
import { useAuthenticationStore } from '@/login/authentication.store';
import { useLogStore } from "@/console-log/log.store";

const authenticationStore = useAuthenticationStore();
const store = useLogStore();

onMounted(() => {
  authenticationStore.getUserData();
});
</script>

<template>
  {{ store.logMessages.length }}
  <RouterView />
</template>