import { ref, computed } from 'vue';
import { defineStore } from 'pinia';
import http from '@/api/http';
import type { IObsRequest, IObsSettings } from './models/obs-interface';
import { useNotificationStore } from '@/common/notifications/notification.store';

export const useObsStore = defineStore('obs-store', () => {
  const notificationStore = useNotificationStore();

  const settings = ref<IObsSettings>({
    websocketUrl: '123',
    port: '404',
    password: '1312231',
    connectionStatus: false
  });

  const saveSettings = async (settings: IObsRequest) => {
    try {
      await http.post('/api/obs', settings);
    } catch (e) {
      notificationStore.displayMessage('Error with saving settings');
    }
  };

  const updateObsConnection = async (enabled: boolean) => {
    console.log('updateobsconnected', enabled);
    try {
      if (enabled) {
        await http.post('/api/obs/connection', null);
        settings.value.connectionStatus = true;
      } else {
        await http.delete('/api/obs/connection');
        settings.value.connectionStatus = false;
      }
    } catch {
      notificationStore.displayMessage('Error with connection');
    }
  };

  return {
    settings,
    saveSettings,
    updateObsConnection
  };
});
