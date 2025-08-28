import {ref} from 'vue';
import {defineStore} from 'pinia';
import {useSignalR} from '@/api/signalr.composable';
import {useNotificationStore} from '@/common/notifications/notification.store';
import type {IStreamInfo, IStreamInfoRequest} from './models/stream-info.interface';
import {useAxios} from "@/api/newhttp";
import type {IStreamSummary} from "@/home/stream-info/models/stream-summary.interface.ts";

export const useStreamInfoStore = defineStore('stream-info', () => {
  const {getConnectionByHub} = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');
  const notificationStore = useNotificationStore();
  const axios = useAxios();

  const streamSummary = ref<IStreamSummary>({id: 0, summary: ''});

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

  const getStreamSummary = async () => {
    const result = await axios.get<IStreamSummary>('/api/stream/summary', null);
    if (result) {
      streamSummary.value = result;
    }
  }

  const updateStreamSummary = async () => {
    await axios.patch('/api/stream/summary', streamSummary.value, null);
  }

  return {
    streamInfo,
    getStreamInfo,
    updateStreamInfo,
    streamSummary,
    getStreamSummary,
    updateStreamSummary
  };
});