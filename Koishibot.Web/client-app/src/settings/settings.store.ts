import { ref, computed } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import http from '@/api/http';
import { useNotificationStore } from '@/common/notification.store';

export const useSettingsStore = defineStore('settingsStore', () => {
  const notificationStore = useNotificationStore();

  const message = ref('');

  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  const UpdateObsConnection = async (enabled: boolean) => {
    try {
      if (enabled) {
        await http.post('/api/obs/', null);
        message.value = 'Obs Websocket Connected';
      } else {
        await http.delete('/api/obs');
        message.value = 'Obs Websocket Disconnected';
      }
    } catch {
      message.value = 'Error with connection';
    }
  };

  return {
    message,
    UpdateObsConnection
  };
});
