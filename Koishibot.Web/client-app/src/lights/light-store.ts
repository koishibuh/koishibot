import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import { type ILedLight } from './models/ledlight-interface';
import { type IColorSquare } from './models/colorsquare-interface';
import sampleLightData from '@/lights/data/sampleLights.json';
import defaultColorData from '@/lights/data/defaultColors.json';
import http from '@/api/http';

export const useLightStore = defineStore('lights', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  const defaultColors = ref<IColorSquare[]>(defaultColorData);
  const currentLights = ref<ILedLight[]>(sampleLightData);
  const message = ref<string>();

  /*   signalRConnection?.on('ReceiveLightUpdate', (lights: ILedLight[]) => {});  */

  const updateSelectedColor = (color: string) => {
    /*  defaultColors.value.map((x) => (x.isActive = false)); */
    const lightToUpdate = currentLights.value.find((x) => x.isSelected === true);
    if (lightToUpdate) {
      lightToUpdate.color = color;
    }
  };

  const lightColorSelected = (lightName: string) => {
    currentLights.value.map((x) => (x.isSelected = false));
    const lightToUpdate = currentLights.value.find((x) => x.lightName === lightName);
    if (lightToUpdate) {
      lightToUpdate.isSelected = true;
    }
  };

  const lightPowerChanged = (lightName: string, state: boolean) => {
    const lightToUpdate = currentLights.value.find((x) => x.lightName === lightName);
    if (lightToUpdate) {
      lightToUpdate.power = state;
    }
  };

  const clearSelected = () => {
    currentLights.value.map((x) => (x.isSelected = false));
  };

  const saveColorChange = async () => {
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
      message.value = 'Something went wrong';
    }
  };

  return {
    currentLights,
    defaultColors,
    updateSelectedColor,
    lightColorSelected,
    lightPowerChanged,
    clearSelected,
    saveColorChange
  };
});
