<script setup lang="ts">
/* import VueColorKit from '@/common/color-picker/ColorPicker.vue'; */
import { ref, onUnmounted } from 'vue';
import ToggleButton from '@/common/toggle-button.vue';
import ColorSquare from '@/lights/ColorSquare.vue';
import LightLabel from '@/lights/LightLabel.vue';
import HiddenField from '@/common/hidden-field/HiddenField.vue';

import { useLightStore } from '@/lights/light-store';
import type { ILightLogin } from './models/lightlogin-interface';

const store = useLightStore();

onUnmounted(() => {
  store.clearSelected();
});

const login = async () => {
  const login: ILightLogin = {
    email: email.value,
    password: password.value
  };

  await store.login(login);
};

const importLights = async () => {
  await store.importLights();
};

const email = ref<string>('');
const password = ref<string>('');
/* const color = ref('#59c7f9');
const suckerCanvas = ref(null);
const suckerArea = ref([]);
const isSucking = ref(false); */
</script>

<template>
  <div class="flex flex-col gap-2">
    <div class="flex items-center">
      <label for="email" class="p-2 w-[60px]">Email:</label>
      <input type="text" v-model="email" id="message" class="text-black" />
    </div>

    <HiddenField field-name="Password" @update="(x) => (password = x)" />
    <button class="primary-button" @click="login">Login</button>
    <button class="primary-button" @click="importLights">Import</button>

    <ToggleButton button1Name="Enable Lights" button2Name="Disable Lights" />
    <div v-if="store.currentLights">
      <LightLabel v-for="item in store.currentLights" :key="item.lightName" :light="item" />
    </div>
    <div class="grid grid-flow-col place-items-center">
      <ColorSquare v-for="item in store.defaultColors" :key="item.name" :colorSquare="item" />
    </div>
  </div>

  <button class="primary-button">Send</button>
  <!--   <VueColorKit @changeColor="color"></VueColorKit> -->
</template>
