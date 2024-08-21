import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import { type IServiceStatus } from '@/layout/models/service-status.interface';
import { useObsStore } from '@/settings/obs.store';
import http from '@/api/http';

export const useNotificationStore = defineStore('notification', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  const obsStore = useObsStore();

  const notificationMessage = ref<string>('');
  const serviceStatuses = ref<IServiceStatus[]>();

  const color = ref<boolean>(false);

  async function delay(miliseconds: number) {
    return new Promise((resolve) => setTimeout(resolve, miliseconds));
  }

  const displayMessage = async (message: string) => {
    notificationMessage.value = message;
    await delay(2000);
    notificationMessage.value = '';
  };

  const displayMessageNew = async (error: boolean,  message: string) => {
    notificationMessage.value = message;
    color.value = error;
    await delay(2000);
    notificationMessage.value = '';
  };

  signalRConnection?.on('ReceiveNotification', (notification: string) => {
    notificationMessage.value = notification;
  });

  signalRConnection?.on('ReceiveStatusUpdate', (status: IServiceStatus) => {
    console.log('ReceiveStatusUpdate', status);

    const index = serviceStatuses.value?.findIndex((x) => x.name === status.name);
    if (index !== undefined && index !== -1 && serviceStatuses.value !== undefined) {
      serviceStatuses.value[index] = status;
    }
    const boolStatus = status.status === 'Offline' ? false : true;

    if (status.name === 'ObsWebsocket') {
      obsStore.settings.connectionStatus = boolStatus;
    }
  });

  const getStatusByName = (serviceName: string): boolean => {
    const result = serviceStatuses.value?.find((x) => x.name === serviceName);
    const boolStatus = result?.status === 'Offline' ? false : true;
    if (result) {
      return boolStatus;
    } else {
      return false;
    }
  };

  const getStatus = async () => {
    serviceStatuses.value = await http.get('/api/service-status');
  };

  return {
    notificationMessage,
    serviceStatuses,
    getStatusByName,
    getStatus,
    displayMessage,
    displayMessageNew,
    color
  };
});