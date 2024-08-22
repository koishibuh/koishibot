import {ref, computed} from 'vue';
import {defineStore} from 'pinia';
import {useSignalR} from '@/api/signalr.composable';
import http from '@/api/http';
import axios from 'axios';
import {useNotificationStore} from '@/common/notifications/notification.store';

export const useStreamElementsStore = defineStore('streamElementsStore', () => {
  const notificationStore = useNotificationStore();

  const saveJwtToken = async (token: string) => {
    await axios.post('api/stream-elements/token', {jwtToken: token});
    await notificationStore.displayMessageNew(false, 'Saved token');
  };

  const connectStreamElements = async () => {
    await axios.post('api/stream-elements', null);
  }

  return {
    saveJwtToken,
    connectStreamElements
  };
});