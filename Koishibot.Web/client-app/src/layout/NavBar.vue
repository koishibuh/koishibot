<script setup lang="ts">
import { RouterLink } from 'vue-router';
import {computed} from "vue";

const isDevMode = computed(() => import.meta.env.MODE === 'development');
</script>

<template>
  <p v-if="isDevMode" class="p-2 bg-red-400 text-center">🛠️ DEV MODE</p>
  <nav class="flex flex-col justify-between mt-[1px]">
    <RouterLink :to="{ name: 'Home' }">Home</RouterLink>
    <RouterLink :to="{ name: 'Attendance' }">Attendance</RouterLink>
    <p>Raid</p>
    <p>Game Jam</p>
    <RouterLink :to="{ name: 'Poll' }">Poll</RouterLink>
    <RouterLink :to="{ name: 'Commands' }">Commands</RouterLink>
    <RouterLink :to="{ name: 'Channel Points' }">Channel Points</RouterLink>
    <RouterLink :to="{ name: 'Lights' }">Lights</RouterLink>
    <RouterLink :to="{ name: 'Dandle' }">Dandle</RouterLink>
    <RouterLink :to="{ name: 'Misc' }">Misc</RouterLink>
    <RouterLink :to="{ name: 'Settings' }">Settings</RouterLink>
    <RouterLink :to="{ name: 'Log' }">Log</RouterLink>
  </nav>
</template>