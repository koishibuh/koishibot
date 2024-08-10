import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import { useNotificationStore } from '@/common/notifications/notification.store';
import type { IStreamInfo, IStreamInfoRequest } from './models/stream-info.interface';
import http from '@/api/http';

export const useStreamInfoStore = defineStore('stream-info', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');
  const notificationStore = useNotificationStore();

  const streamInfo = ref<IStreamInfo>();

  signalRConnection?.on('ReceiveStreamInfo', (info: IStreamInfo) => {
    streamInfo.value = info;
  });

  const getStreamInfo = async () => {
    try {
      streamInfo.value = await http.get('/api/stream-info/twitch');
    } catch (error) {
      notificationStore.displayMessage((error as Error).message);
    }
  };

  const updateStreamInfo = async (request: IStreamInfoRequest) => {
    try {
      streamInfo.value = await http.post('/api/stream-info/twitch', request);
    } catch (error) {
      notificationStore.displayMessage((error as Error).message);
    }
  };

  return {
    streamInfo,
    getStreamInfo,
    updateStreamInfo
  };
});
