import { ref, computed } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import http from '@/api/http';
import { useNotificationStore } from '@/common/notifications/notification.store';
import { type ILog } from '@/settings/models/log-interface';

export const useSettingsStore = defineStore('settingsStore', () => {
  const notificationStore = useNotificationStore();

  const logMessages = ref<ILog[]>([]);

  const message = ref('');

  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  signalRConnection?.on('ReceiveLog', (log: ILog) => {
    logMessages.value.push(log);
  });

  /* const UpdateObsConnection = async (enabled: boolean) => {
    console.log('test');
    try {
      if (enabled) {
        await http.post('/api/obs', null);
        message.value = 'Obs Websocket Connected';
      } else {
        await http.delete('/api/obs');
        message.value = 'Obs Websocket Disconnected';
      }
    } catch {
      message.value = 'Error with connection';
    }
  };
 */
  return {
    message,
    logMessages
    /*     UpdateObsConnection */
  };
});
