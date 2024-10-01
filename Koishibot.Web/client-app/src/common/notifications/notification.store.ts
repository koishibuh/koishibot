import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import { type IServiceStatus } from '@/layout/models/service-status.interface';
import { useObsStore } from '@/settings/obs.store';
import http from '@/api/http';
import { type INotification } from "@/common/notifications/notification.interface";

export const useNotificationStore = defineStore('notification', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  const obsStore = useObsStore();

  const notificationMessage = ref<string>('');
  const serviceStatuses = ref<IServiceStatus[]>();

  const color = ref<boolean>(false);

  async function delay(seconds: number) {
    const milliseconds: number = seconds * 1000;
    return new Promise((resolve) => setTimeout(resolve, milliseconds));
  }

  const displayMessage = async (message: string) => {
    notificationMessage.value = message;
    await delay(2);
    notificationMessage.value = '';
  };

  const displayMessageNew = async (error: boolean,  message: string) => {
    notificationMessage.value = message;
    color.value = error;
    await delay(2);
    notificationMessage.value = '';
  };

  const displayErrorMessage = async (message : string) => {
    notificationMessage.value = message;
    color.value = true;
    await delay(2);
    notificationMessage.value = '';
  }

  signalRConnection?.on('ReceiveNewNotification', async (notification: INotification) => {
    color.value = notification.error;
    notificationMessage.value = notification.message;
    await delay(2);
    notificationMessage.value = '';
  })

  signalRConnection?.on('ReceiveNotification', (notification: string) => {
    notificationMessage.value = notification;
  });

  signalRConnection?.on('ReceiveStatusUpdate', (status: IServiceStatus) => {
    console.log('ReceiveStatusUpdate', status);

    const index = serviceStatuses.value?.findIndex((x) => x.name === status.name);
    if (index !== undefined && index !== -1 && serviceStatuses.value !== undefined) {
      serviceStatuses.value[index] = status;
    }
    const boolStatus = status.status !== 'Offline';

    if (status.name === 'ObsWebsocket') {
      obsStore.settings.connectionStatus = boolStatus;
    }
  });

  const getStatusByName = (serviceName: string): boolean => {
    const result = serviceStatuses.value?.find((x) => x.name === serviceName);
    const boolStatus = result?.status !== 'Offline';
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
    displayErrorMessage,
    color,
    delay
  };
});