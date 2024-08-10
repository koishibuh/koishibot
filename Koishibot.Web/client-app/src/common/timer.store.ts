import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import { type IOverlayTimer } from '@/timers/models/timer.interface';

export const useTimerStore = defineStore('timers', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  const defaultTimer: IOverlayTimer = { title: '', minutes: 0, seconds: 0 };

  const currentOverlayTimer = ref<IOverlayTimer>(defaultTimer);

  signalRConnection?.on('ReceiveOverlayTimer', (overlayTimer: IOverlayTimer) => {
    console.log(overlayTimer.title);
    console.log(overlayTimer.minutes);
    console.log(overlayTimer.seconds);
    currentOverlayTimer.value = overlayTimer;
  });

  return {
    currentOverlayTimer
  };
});
