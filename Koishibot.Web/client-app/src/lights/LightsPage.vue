<script setup lang="ts">
/* import VueColorKit from '@/common/color-picker/ColorPicker.vue'; */
import { ref, onUnmounted } from 'vue';
import ToggleButton from '@/common/toggle-button.vue';
import ColorSquare from '@/lights/ColorSquare.vue';
import LightLabel from '@/lights/LightLabel.vue';

import { useLightStore } from '@/lights/light-store';

const store = useLightStore();

onUnmounted(() => {
  store.clearSelected();
});

/* const color = ref('#59c7f9');
const suckerCanvas = ref(null);
const suckerArea = ref([]);
const isSucking = ref(false); */
</script>

<template>
  <div class="flex flex-col gap-2">
    <button class="primary-button">Login</button>
    <button class="primary-button">Import</button>
    <ToggleButton button1Name="Enable Lights" button2Name="Disable Lights" />
    <LightLabel v-for="item in store.currentLights" :key="item.lightName" :light="item" />
    <div class="grid grid-flow-col place-items-center">
      <ColorSquare v-for="item in store.defaultColors" :key="item.name" :colorSquare="item" />
    </div>
  </div>

  <button class="primary-button">Send</button>
  <!--   <VueColorKit @changeColor="color"></VueColorKit> -->
</template>
