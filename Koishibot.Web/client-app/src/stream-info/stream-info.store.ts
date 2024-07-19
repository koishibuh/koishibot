import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import { type IStreamInfo } from './stream-info.interface';
import http from '@/api/http';

export const useStreamInfoStore = defineStore('stream-info', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  const streamInfo = ref<IStreamInfo>();

  signalRConnection?.on('ReceiveStreamInfo', (info: IStreamInfo) => {
    streamInfo.value = info;
  });

  async function GetStreamInfo() {
    streamInfo.value = await http.get('/api/stream-info/twitch');
  }

  async function UpdateStreamInfo(streamTitle: string | undefined, category: string | undefined) {
    streamInfo.value = await http.post('/api/stream-info/twitch', {
      title: streamTitle,
      category: category
    });
  }

  return {
    streamInfo,
    GetStreamInfo,
    UpdateStreamInfo
  };
});
