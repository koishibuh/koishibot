<script setup lang="ts">
import calendar from '@/calendars/data/calendarData.json';
</script>

<template>
  <div class="w-1/3 text-4xl">
    <div class="flex bg-gradient-to-r from-blue-400 to-purple-500 justify-center p-2 rounded-t">
      <p class="uppercase text-3xl text-white font-bold">Stream Schedule Twitch.tv/elysiagriffin</p>
    </div>

    <div v-for="(item, index) in calendar" :key="index" class="flex bg-gray-200 mb-1">
      <div class="flex flex-col bg-pink-400 w-1/4 uppercase">
        <p>{{ item.Day }}</p>
        <p>{{ item.Date }}</p>
      </div>
      <div class="w-2/4 uppercase text-center text-balance bg-g flex flex-col justify-center">
        <p class="bg-b">{{ item.Content }}</p>
      </div>
      <div class="flex flex-col bg-pink-400 w-1/4 uppercase">
        <p>{{ item.TimeUK }}</p>
        <p>{{ item.TimeEst }}</p>
      </div>
    </div>
  </div>
</template>
