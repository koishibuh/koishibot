import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';

export const useAdTimerStore = defineStore('adTimerStore', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  /*   const adTimer = ref<number>(18000); */
  const adTimer = ref<IAdTimer>();

  signalRConnection?.on('ReceiveAdStartedEvent', (ad: IAdTimer) => {
    adTimer.value = ad;
  });

  return {
    adTimer
  };
});

interface IAdTimer {
  adLength: number;
  timerEnds: Date;
}
