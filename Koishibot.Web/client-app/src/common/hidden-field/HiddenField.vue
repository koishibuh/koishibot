<script setup lang="ts">
import { ref, computed, watch } from 'vue';

const props = defineProps<{
  fieldName: string;
}>();

const emit = defineEmits<{
  (e: 'update', value: string): void;
}>();

const textField = ref('');
const isTextVisible = ref(false);

const toggleVisibility = (visible: boolean) => {
  isTextVisible.value = visible;
};

const inputType = computed(() => {
  return isTextVisible.value ? 'text' : 'password';
});

const createLabelName = computed(() => {
  return props.fieldName.toLowerCase();
});

watch(textField, (newValue: string) => {
  emit('update', newValue);
});
</script>

<template>
  <div class="flex items-center justify-between">
    <label :for="createLabelName" class="p-2 w-1/3">{{ fieldName }}:</label>
    <input :type="inputType" v-model="textField" id="address" class="text-black w-2/3" />
    <button
      @click.prevent=""
      class="primary-button mx-2 w-1/3"
      @mousedown="toggleVisibility(true)"
      @mouseup="toggleVisibility(false)"
      @mouseleave="toggleVisibility(false)"
    >
      Show
    </button>
  </div>
</template>
