<script setup lang="ts">
import { ref } from 'vue';
import { type IColorSquare } from './models/colorsquare-interface';
import { useLightStore } from '@/lights/light-store';

const props = defineProps<{
  colorSquare: IColorSquare;
}>();

const store = useLightStore();

const updateColorSelected = () => {
  store.updateSelectedColor(props.colorSquare.color);
};
</script>

<template>
  <div class="w-10 h-10 flex justify-center items-center mb-2">
    <div
      class="w-8 h-8 rounded hover:w-10 hover:h-10"
      :class="{ 'border-2': colorSquare.isActive, 'border-none': !colorSquare.isActive }"
      :style="{ backgroundColor: colorSquare.color }"
      @click="updateColorSelected()"
    ></div>
  </div>
</template>
