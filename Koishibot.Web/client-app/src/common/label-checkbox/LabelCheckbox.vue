<script setup lang="ts">
import { ref } from 'vue';

const props = defineProps<{
  item: { name: string; selected: boolean };
}>();

const checked = ref<boolean>(props.item.selected);
</script>

<template>
  <div class="flex items-center gap-2">
    <input type="checkbox" id="checkbox" v-model="checked" />
    <div>{{ item.name }}</div>
  </div>
</template>
