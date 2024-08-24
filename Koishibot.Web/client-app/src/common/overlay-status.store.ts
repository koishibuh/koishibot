import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import type { IOverlayStatus } from './overlay-status.interface';
import {useAxios} from "@/api/newhttp";

export const useOverlayStatusStore = defineStore('overlayStatusStore', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');
  const http = useAxios();

  const dandleOverlayStatus = ref<boolean>(false);

  signalRConnection?.on('ReceiveOverlayStatus', (overlayStatus: IOverlayStatus) => {
    if (overlayStatus.name === 'Dandle') {
      dandleOverlayStatus.value = overlayStatus.status;
    }
  });
  return {
    dandleOverlayStatus
  };
});