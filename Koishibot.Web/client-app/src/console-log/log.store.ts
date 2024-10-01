import { ref, computed } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import http from '@/api/http';
import { useNotificationStore } from '@/common/notifications/notification.store';
import { type ILog } from '@/settings/models/log-interface';

export const useLogStore = defineStore('logStore', () => {
  const notificationStore = useNotificationStore();
  const logMessages = ref<ILog[]>([]);

  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  signalRConnection?.on('ReceiveInfo', (log: ILog) => {
    notificationStore.displayMessage(log.message);
    logMessages.value.push(log);
  });

  signalRConnection?.on('ReceiveError', (log: ILog) => {
    notificationStore.displayErrorMessage(log.message);
    logMessages.value.push(log);
  });

  signalRConnection?.on('ReceiveLog', (log: ILog) => {
    logMessages.value.push(log);
  });

  return {
    logMessages
  };
});