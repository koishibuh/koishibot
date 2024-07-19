<script setup lang="ts">
import { ref, watch, computed } from 'vue';

var props = defineProps({
  button1Name: {
    type: String,
    default: 'Button 1'
  },
  button2Name: {
    type: String,
    default: 'Button 2'
  },
  state: {
    type: Boolean
  }
});

/* const isButtonEnabled = ref<boolean>(true); */

const emit = defineEmits<{
  updateState: [state: boolean];
}>();

const enableItem = () => {
  emit('updateState', true);
};

const disableItem = () => {
  emit('updateState', false);
};
</script>

<template>
  <div class="w-full">
    <button class="toggle-button-l" @click="enableItem" :disabled="state">
      {{ button1Name }}
    </button>
    <button class="toggle-button-r" @click="disableItem" :disabled="!state">
      {{ button2Name }}
    </button>
  </div>
</template>
