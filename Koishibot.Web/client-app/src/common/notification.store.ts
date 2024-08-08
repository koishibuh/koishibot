import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import { type IServiceStatus } from '@/layout/service-status.interface';
import { useObsStore } from '@/settings/obs.store';
import http from '@/api/http';

export const useNotificationStore = defineStore('notification', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  const obsStore = useObsStore();

  const notificationMessage = ref('');
  const serviceStatuses = ref<IServiceStatus[]>();

  signalRConnection?.on('ReceiveNotification', (notification: string) => {
    console.log('ReceiveNotification', notification);
    notificationMessage.value = notification;
  });

  signalRConnection?.on('ReceiveStatusUpdate', (status: IServiceStatus) => {
    console.log('ReceiveStatusUpdate', status);
    /* let result = serviceStatuses.value?.find(x => x.name === status.name);
		result = status; */

    const index = serviceStatuses.value?.findIndex((x) => x.name === status.name);
    if (index !== undefined && index !== -1 && serviceStatuses.value !== undefined) {
      serviceStatuses.value[index] = status;
    }

    if (status.name === 'ObsWebsocket') {
      obsStore.settings.connectionStatus = status.status;
    }
  });

  function getStatusByName(serviceName: string): boolean {
    const result = serviceStatuses.value?.find((x) => x.name === serviceName);
    if (result) {
      return result.status;
    } else {
      return false;
    }
  }

  async function GetStatus() {
    serviceStatuses.value = await http.get('/api/service-status');
  }

  return {
    GetStatus,
    notificationMessage,
    serviceStatuses,
    getStatusByName
  };
});
