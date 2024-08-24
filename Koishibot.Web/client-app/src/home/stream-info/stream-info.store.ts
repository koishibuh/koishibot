import {ref} from 'vue';
import {defineStore} from 'pinia';
import {useSignalR} from '@/api/signalr.composable';
import {useNotificationStore} from '@/common/notifications/notification.store';
import type {IStreamInfo, IStreamInfoRequest} from './models/stream-info.interface';
import {useAxios} from "@/api/newhttp";

export const useStreamInfoStore = defineStore('stream-info', () => {
  const {getConnectionByHub} = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');
  const notificationStore = useNotificationStore();
  const axios = useAxios();

  const streamInfo = ref<IStreamInfo>({streamTitle: '', category: '', categoryId: ''});

  signalRConnection?.on('ReceiveStreamInfo', (info: IStreamInfo) => {
    streamInfo.value = info;
  });

  const getStreamInfo = async () => {
    const response = await axios.get<IStreamInfo>('/api/stream-info/twitch', null);
    if (response) {
      streamInfo.value = response;
    }
  };

  const updateStreamInfo = async (request: IStreamInfoRequest) => {
    streamInfo.value = await axios.post('/api/stream-info/twitch', request);
    await notificationStore.displayMessageNew(false, "Sent");
  };

  return {
    streamInfo,
    getStreamInfo,
    updateStreamInfo
  };
});