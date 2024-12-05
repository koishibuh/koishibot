import {ref, computed} from 'vue';
import {defineStore} from 'pinia';
import http from '@/api/http';
import type {IObsRequest, IObsSettings} from './models/obs-interface';
import {useNotificationStore} from '@/common/notifications/notification.store';
import {useAxios} from "@/api/newhttp";
import {useSignalR} from "@/api/signalr.composable";
import type {IPoll} from "@/polls/models/poll.interface";
import {data} from "autoprefixer";

export const useObsStore = defineStore('obs-store', () => {
  const notificationStore = useNotificationStore();
  const axios = useAxios();

  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  const settings = ref<IObsSettings>({
    websocketUrl: '',
    port: '4455',
    password: '',
    connectionStatus: false
  });

  const saveSettings = async (settings: IObsRequest) => {
    try {
      await http.post('/api/obs', settings);
    } catch (error) {
      await notificationStore.displayErrorMessage((error as Error).message);
    }
  };

  const updateObsConnection = async (enabled: boolean) => {
    try {
      if (enabled) {
        await axios.post('/api/obs/connection', null);
        settings.value.connectionStatus = true;
      } else {
        await axios.remove('/api/obs/connection', null);
        settings.value.connectionStatus = false;
      }
    } catch (error) {
      await notificationStore.displayErrorMessage((error as Error).message);
    }
  };

  const obsItemList = ref<IObsItem[]>([]);

  const importObsInputs = async () => {
    try {
      await axios.get('/api/obs/input', null)
    } catch (error) {
      await notificationStore.displayErrorMessage((error as Error).message);
    }
  }

  signalRConnection?.on('ReceiveObsItems', (response: IObsItem[]) => {
    // let itemMap = new Map(obsItemList.value.map(item => [item.Id,  item]));
    //
    // response.forEach(x => {
    //   itemMap.set(x.Id, x);
    // });
    //
    // obsItemList.value = Array.from(itemMap.values());
    obsItemList.value = response;
  });

  const updateObsItemAppName = async (obsItem: IObsAppName) => {
    try {
      console.log(obsItem.appName);
      await axios.patch('/api/obs/item', { appName: obsItem.appName }, { id: obsItem.id } );
    } catch (error) {
      await notificationStore.displayErrorMessage((error as Error).message);
    }
  }


  return {
    settings,
    saveSettings,
    updateObsConnection,
    importObsInputs,
    obsItemList,
    updateObsItemAppName
  };
});

export interface IObsItem {
  id: number;
  type: string;
  obsName: string;
  appName: string | null;
}

export interface IObsAppName {
  id: number;
  appName: string;
}