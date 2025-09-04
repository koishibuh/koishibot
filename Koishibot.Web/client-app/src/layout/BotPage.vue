<script setup lang="ts">
import { RouterLink, RouterView, useRoute } from 'vue-router';
import { watch, onMounted, ref } from 'vue';
import ChatPage from '@/chat/ChatPage.vue';
import FeedPage from '@/event-feed/FeedPage.vue';
import NavBar from './NavBar.vue';
import ChatField from '@/chat/ChatField.vue';
import ServiceStatusBar from './ServiceStatusBar.vue';
import NotificationBanner from '@/common/notifications/NotificationBanner.vue';
import { useNotificationStore } from '@/common/notifications/notification.store';

const route = useRoute();
const store = useNotificationStore();

</script>

<template>
  <div class="flex flex-col relative max-w-[772px] m-auto bg-[#1f212a]">
    <div class="flex sticky top-0 h-[53vh] border-[#1f212a] border-b-2">
      <div class="w-3/5 bg-primary flex flex-col relative rounded-bl rounded-tl mt-1">
        <ChatPage />

        <ChatField />
      </div>

      <div class="bg-secondary w-2/5 rounded-br rounded-tr mt-1 overflow-auto">
        <FeedPage />
      </div>

      <div class="absolute top-[53vh] h-[47vh] w-[114px] bg-secondary rounded overflow-auto">
        <NavBar />
      </div>
    </div>
    <div id="bottom" class="flex h-[47vh] bg-[#1f212a] gap-2">
      <div class="w-[133px]"></div>
      <div class="flex flex-col w-full">
        <div
          class="bg-accent-two h-[24px] rounded-t flex items-center gap-2 justify-center relative"
        >
          <h1>{{ route.name }}</h1>
          <NotificationBanner :error="store.color" :message="store.notificationMessage" />
        </div>

        <main class="w-[650px] flex flex-col h-[44.5vh] overflow-auto bg-background p-3">
          <RouterView />
        </main>
        <div class="bg-accent-two h-[24px] rounded-b flex items-center gap-2 justify-center">
          <ServiceStatusBar/>
        </div>
      </div>
    </div>
  </div>
</template>