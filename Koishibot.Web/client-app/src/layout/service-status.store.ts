import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import type { IServiceStatus } from '@/layout/models/service-status.interface';
import { useObsStore } from '@/settings/obs.store';
import { useNotificationStore } from '@/common/notifications/notification.store';
import { useTwitchStore } from '@/settings/twitch.store';
import http from '@/api/http';

export const useServiceStatusStore = defineStore('serviceStatus', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  const notificationStore = useNotificationStore();
  const obsStore = useObsStore();
  const twitchStore = useTwitchStore();

  const serviceStatuses = ref<IServiceStatus[]>();

  signalRConnection?.on('ReceiveStatusUpdate', (status: IServiceStatus) => {
    const index = serviceStatuses.value?.findIndex((x) => x.name === status.name);
    if (index !== undefined && index !== -1 && serviceStatuses.value !== undefined) {
      serviceStatuses.value[index] = status;
    }

    switch (status.name) {
      case 'ObsWebsocket':
        obsStore.settings.connectionStatus = status.status;
        break;
      case 'BotIrc':
        twitchStore.ircStatus = status.status;
        break;
      case 'TwitchWebsocket':
        twitchStore.eventSubStatus = status.status;
        break;
      default:
        break;
    }
  });

  /*   const getStatusByName = (serviceName: string): boolean => {
    try {
      const result = serviceStatuses.value?.find((x) => x.name === serviceName);
      if (result) {
        return result.status;
      } else {
        return false;
      }
    } catch (error) {
      notificationStore.displayMessage('Error getting status');
    }
  }; */

  const getStatus = async () => {
    try {
      serviceStatuses.value = await http.get('/api/service-status');
    } catch (error) {
      notificationStore.displayMessage('Error getting status');
    }
  };

  return {
    serviceStatuses,
    /*     getStatusByName, */
    getStatus
  };
});
