import { ref, computed } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import { useNotificationStore } from '@/common/notifications/notification.store';
import { type ILog } from '@/settings/models/log-interface';

export const useLogStore = defineStore('logStore', () => {
  const notificationStore = useNotificationStore();
  const logMessages = ref<ILog[]>([]);

  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  signalRConnection?.on('ReceiveInfo', async (log: ILog) => {
    await notificationStore.displayMessage(log.message);
    logMessages.value.push(log);
  });

  signalRConnection?.on('ReceiveError', async (log: ILog) => {
    await notificationStore.displayErrorMessage(log.message);
    logMessages.value.push(log);
  });

  signalRConnection?.on('ReceiveLog', (log: ILog) => {
    logMessages.value.push(log);
  });

  return {
    logMessages
  };
});