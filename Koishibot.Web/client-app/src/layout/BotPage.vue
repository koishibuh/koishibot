<script setup lang="ts">
import { RouterLink, RouterView, useRoute } from 'vue-router';
import { watch, onMounted, ref } from 'vue';
import ChatPage from '@/chat/ChatPage.vue';
import FeedPage from '@/event-feed/FeedPage.vue';
import RecentEvent from '@/event-feed/RecentEvent.vue';
import NavBar from './NavBar.vue';
import ChatField from '@/chat/ChatField.vue';
import ServiceStatusBar from './ServiceStatusBar.vue';

const route = useRoute();
</script>

<template>
  <!--   <div class="flex flex-col bg-primary h-screen">
    <div class="">
      <NavBar />
    </div>

    <main class="flex flex-col lg:flex-row lg:h-5/6 grow">
      <div id="chat-box" class="bg-secondary rounded-lg h-[500px] w-full m-1 flex flex-col">
        <div class="">
          <RouterLink :to="{ name: 'Feed' }"> <RecentEvent /></RouterLink>
        </div>

        <ChatPage />
        <div class="">
          <ChatField />
        </div>
      </div>

      <div id="main-box" class="bg-background rounded-lg w-full m-1 flex flex-col">
        <div class="bg-accent-two h-[24px] rounded-t-lg flex items-center gap-2 justify-center">
          <h1>{{ route.name }}</h1>
        </div>

        <div class="h-[100%] overflow-y-scroll w-full p-2">
          <RouterView />
        </div>

        <ServiceStatusBar />
      </div>
    </main>
  </div>
 -->
  <!-- <div class="flex">
    <div class="flex h-[500px] sticky top-0">
      <div class="bg-b max-w-md"><ChatPage /></div>
      <div class="bg-red-500 w-[42%]">Event</div>
    </div>
    <div class="flex">
      <div class="sticky top-0">NAVBAR</div>
      <div class="bg-g basis-full">
        <p class="text-7xl">Page</p>
        <p class="text-7xl">Page</p>
        <p class="text-7xl">Page</p>
        <p class="text-7xl">Page</p>
        <p class="text-7xl">Page</p>
        <p class="text-7xl">Page</p>
        <p class="text-7xl">Page</p>
        <p class="text-7xl">Page</p>
        <p class="text-7xl">Page</p>
        <p class="text-7xl">Page</p>
        <p class="text-7xl">Page</p>
        <p class="text-7xl">Page</p>
        <p class="text-7xl">Page</p>
        <p class="text-7xl">Paee</p>
      </div>
    </div>
  </div> -->

  <div class="flex flex-col relative max-w-[772px] m-auto bg-[#1f212a]">
    <div class="flex sticky top-0 h-[53vh] border-[#1f212a] border-b-2">
      <div class="w-3/5 bg-primary flex flex-col relative rounded-bl rounded-tl mt-1">
        <ChatPage />

        <ChatField />
      </div>

      <div class="bg-secondary w-2/5 rounded-br rounded-tr mt-1 overflow-auto">
        <FeedPage />
      </div>

      <div class="absolute top-[53vh] h-[47vh] w-[114px] bg-secondary rounded"><NavBar /></div>
    </div>
    <div id="bottom" class="flex h-[47vh] bg-[#1f212a]">
      <div class="w-[114px]"></div>
      <div class="flex flex-col ml-2">
        <div class="bg-accent-two h-[24px] rounded-t flex items-center gap-2 justify-center">
          <h1>{{ route.name }}</h1>
        </div>
        <main class="w-[650px] flex flex-col h-[44.5vh] overflow-auto bg-background p-3">
          <RouterView />
        </main>
        <div class="bg-accent-two h-[24px] rounded-b flex items-center gap-2 justify-center">
          <ServiceStatusBar />
        </div>
      </div>
      <!--     <div class="bg-secondary h-min-[47vh] w-full self-end">
        <div class="">
          <RouterView /> -->

      <!--         <p class="text-7xl">Page</p>
          <p class="text-7xl">Page</p>
          <p class="text-7xl">Page</p>
          <p class="text-7xl">Page</p>
          <p class="text-7xl">Page</p>
          <p class="text-7xl">Page</p>
          <p class="text-7xl">Page</p>
          <p class="text-7xl">Page</p>
          <p class="text-7xl">Page</p>
          <p class="text-7xl">Page</p>
          <p class="text-7xl">Page</p>
          <p class="text-7xl">Page</p>
          <p class="text-7xl">Page</p>
          <p class="text-7xl">Bard</p> -->
      <!--  </div>
        <div class="bg-primary"><ServiceStatusBar /></div> -->
      <!-- </div> -->
    </div>
  </div>
</template>
