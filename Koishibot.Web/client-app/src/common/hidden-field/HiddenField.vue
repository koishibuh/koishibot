<script setup lang="ts">
import { ref, computed, watch } from 'vue';

const emit = defineEmits<{
  (e: 'update', value: string): void;
}>();

const address = ref('');

const isTextVisible = ref(false);

const inputType = computed(() => {
  return isTextVisible.value ? 'text' : 'password';
});

const toggleVisibility = (visible: boolean) => {
  isTextVisible.value = visible;
};

watch(address, (newValue: string) => {
  emit('update', newValue);
});
</script>

<template>
  <div class="flex items-center justify-between">
    <label for="address" class="p-2 w-[60px]">Address:</label>
    <input :type="inputType" v-model="address" id="address" class="text-black" />
    <button
      @click.prevent=""
      class="primary-button mx-2"
      @mousedown="toggleVisibility(true)"
      @mouseup="toggleVisibility(false)"
      @mouseleave="toggleVisibility(false)"
    >
      Show
    </button>
  </div>
</template>
