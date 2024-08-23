import { ref, computed } from 'vue';
import { defineStore } from 'pinia';
import http from '@/api/http';
import type { IObsRequest, IObsSettings } from './models/obs-interface';
import { useNotificationStore } from '@/common/notifications/notification.store';
import {useAxios} from "@/api/newhttp";

export const useObsStore = defineStore('obs-store', () => {
  const notificationStore = useNotificationStore();
  const axios = useAxios();

  const settings = ref<IObsSettings>({
    websocketUrl: '',
    port: '4455',
    password: '',
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
    try {
      if (enabled) {
        // await http.post('/api/obs/connection', null);
        await axios.post('/api/obs/connection', null);
        settings.value.connectionStatus = true;
      } else {
        await axios.remove('/api/obs/connection', null);
        // await http.delete('/api/obs/connection');
        settings.value.connectionStatus = false;
      }
    } catch (error) {

        // notificationStore.displayMessage(error.response);

    }
  };

  return {
    settings,
    saveSettings,
    updateObsConnection
  };
});