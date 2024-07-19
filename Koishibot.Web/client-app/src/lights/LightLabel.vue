<script setup lang="ts">
import { ref, watch } from 'vue';
import { type ILedLight } from './models/ledlight-interface';
import ToggleButton from '@/common/toggle-button.vue';
import { useLightStore } from './light-store';

const props = defineProps<{
  light: ILedLight;
}>();

const store = useLightStore();

/* const emit = defineEmits<{
  lightColorSelected: [light: ILedLight];
  lightPowerChanged: [light: ILedLight];
}>();
 */

const lightColorSelected = () => {
  store.lightColorSelected(props.light.lightName);
};

const lightPowerChanged = (state: boolean) => {
  store.lightPowerChanged(props.light.lightName, state);
};
</script>

<template>
  <div class="flex items-center border rounded p-2">
    <div class="w-full align-middle">{{ light.lightName }}</div>
    <div
      class="h-5 w-1/2 rounded hover:border-2"
      :class="{ 'border-2': light.isSelected, 'border-none': !light.isSelected }"
      :style="{ backgroundColor: light.color }"
      @click="lightColorSelected"
    ></div>
    <ToggleButton
      class="flex justify-center"
      button1Name="Enable"
      button2Name="Disable"
      :state="light.power"
      @update-state="lightPowerChanged"
    />
  </div>
</template>
