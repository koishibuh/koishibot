import { ref, computed } from 'vue';
import { defineStore } from 'pinia';
import http from '@/api/http';
import type { IObsRequest, IObsSettings } from './models/obs-interface';

export const useObsStore = defineStore('obs-store', () => {
  const message = ref('');

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
      message.value = 'Error with saving settings';
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
      message.value = 'Error with connection';
    }
  };

  return {
    settings,
    message,
    saveSettings,
    updateObsConnection
  };
});
