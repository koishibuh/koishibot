<script setup lang="ts">
import { ref, getCurrentInstance } from 'vue';

const emit = defineEmits<{
  settingClicked: [string];
}>();

const showModal = () => {
  emit('settingClicked', 'ScopesSettings');
};
</script>

<template>
  List of Scopes or something
  <div class="w-1/2 mt-3">
    <button class="primary-button w-full" @click="showModal">Settings</button>
  </div>
</template>
