import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import { type IAdTimer } from './models/timer.interface';

export const useAdTimerStore = defineStore('adTimerStore', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  const adTimer = ref<IAdTimer>({ adLength: 180000, timerEnds: new Date(Date.now())});

  signalRConnection?.on('ReceiveAdStartedEvent', (ad: IAdTimer) => {
    adTimer.value = ad;
  });

  return {
    adTimer
  };
});