<script setup lang="ts">
import { RouterLink } from 'vue-router';
</script>

<template>
  <nav class="flex flex-col justify-between mt-[1px]">
    <RouterLink :to="{ name: 'Home' }">Home</RouterLink>
    <p>Attendance</p>
    <p>Raid</p>
    <p>Game Jam</p>
    <RouterLink :to="{ name: 'Poll' }">Poll</RouterLink>
    <RouterLink :to="{ name: 'Commands' }">Commands</RouterLink>
    <RouterLink :to="{ name: 'Channel Points' }">Channel Points</RouterLink>
    <RouterLink :to="{ name: 'Lights' }">Lights</RouterLink>
    <RouterLink :to="{ name: 'Dandle' }">Dandle</RouterLink>
    <RouterLink :to="{ name: 'Settings' }">Settings</RouterLink>
  </nav>
</template>
