import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import { type ILedLight } from './models/ledlight-interface';
import { type IColorSquare } from './models/colorsquare-interface';
import sampleLightData from '@/lights/sample-data/lightsData.json';
import defaultColorData from '@/lights/default/colorsDefault.json';
import http from '@/api/http';
import { useNotificationStore } from '@/common/notifications/notification.store';
import { type ILightLogin } from './models/lightlogin-interface';

export const useLightStore = defineStore('lights', () => {
  const notificationStore = useNotificationStore();

  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  const defaultColors = ref<IColorSquare[]>(defaultColorData);
  /*   const currentLights = ref<ILedLight[]>(sampleLightData); */
  const currentLights = ref<ILedLight[] | null>(null);

  /*   signalRConnection?.on('ReceiveLightUpdate', (lights: ILedLight[]) => {});  */

  const login = async (login: ILightLogin) => {
    try {
      await http.post('/api/led-lights/login', login);
    } catch (error) {
      notificationStore.displayMessage('Unable to login');
    }
  };

  const importLights = async () => {
    try {
      console.log('importing..');
      const result = await http.get<ILedLight[]>('/api/led-lights/import');
      console.log(result);
      currentLights.value = result;
    } catch (error) {
      notificationStore.displayMessage('Unable to import');
    }
  };

  const updateSelectedColor = (color: string) => {
    if (currentLights.value === null) {
      return;
    }
    const lightToUpdate = currentLights.value.find((x) => x.isSelected === true);
    if (lightToUpdate) {
      lightToUpdate.color = color;
    }
  };

  const lightColorSelected = (lightName: string) => {
    if (currentLights.value === null) {
      return;
    }
    currentLights.value.map((x) => (x.isSelected = false));
    const lightToUpdate = currentLights.value.find((x) => x.lightName === lightName);
    if (lightToUpdate) {
      lightToUpdate.isSelected = true;
    }
  };

  const lightPowerChanged = async (lightName: string, state: boolean) => {
    console.log(state);
    if (currentLights.value === null) {
      return;
    }
    try {
      if (state === true) {
        const result = await http.post('/api/led-lights', { LightName: lightName });
      } else {
        const result = await http.delete('/api/led-lights');
      }

      const lightToUpdate = currentLights.value.find((x) => x.lightName === lightName);
      if (lightToUpdate) {
        lightToUpdate.power = state;
      }
    } catch (error) {
      notificationStore.displayMessage('Unable to update light state');
    }
  };

  const clearSelected = () => {
    if (currentLights.value === null) {
      return;
    }
    currentLights.value.map((x) => (x.isSelected = false));
  };

  const saveColorChange = async () => {
    if (currentLights.value === null) {
      return;
    }
    const dto = currentLights.value.map((x) => {
      return {
        lightName: x.lightName,
        color: x.color,
        power: x.power
      };
    });
    try {
      await http.post('/api/led-lights/', dto);
    } catch (e) {
      notificationStore.displayMessage('Something went wrong');
    }
  };

  return {
    currentLights,
    defaultColors,
    updateSelectedColor,
    lightColorSelected,
    lightPowerChanged,
    clearSelected,
    saveColorChange,
    login,
    importLights
  };
});
