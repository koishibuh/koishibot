<script setup lang="ts">
import { ref, computed, watch, defineEmits } from 'vue';
import { type IDropdownMenu } from './dropdownmenu-interface';

const selected = ref<any>();
const optionsShown = ref(false);
const searchFilter = ref('');

const props = defineProps<{ menu: IDropdownMenu }>();
const emits = defineEmits(['optionSelected', 'filter']);

const selectOption = (option: any) => {
  if (option) {
    console.log('1');
    selected.value = option;
    optionsShown.value = false;
    searchFilter.value = option.name;
    emits('optionSelected', selected.value);
  }
};

const showOptions = () => {
  if (!props.menu.disabled) {
    searchFilter.value = '';
    optionsShown.value = true;
  }
};

const exit = () => {
  optionsShown.value = false;
  console.log(selected.value);
};

const keyMonitor = (event: any) => {
  if (event.key === 'Enter' && filteredOptions.value[0]) {
    selectOption(filteredOptions.value[0]);
    return;
  }
};

/* const arrowUp = (event: any) => {
  let newIndex: number;

  console.log('arrowup');
  newIndex =
    (filteredOptions.value.indexOf(selected.value) - 1 + filteredOptions.value.length) %
    filteredOptions.value.length;

  selectOption(filteredOptions.value[newIndex]);
};

const arrowDown = (event: any) => {
  let newIndex: number;
  newIndex = (filteredOptions.value.indexOf(selected.value) + 1) % filteredOptions.value.length;

  selectOption(filteredOptions.value[newIndex]);
  console.log('arrowdown');
}; */

const filteredOptions = computed(() => {
  const filtered = [];
  const regOption = new RegExp(searchFilter.value, 'ig');

  for (const option of props.menu.options) {
    if (searchFilter.value.length < 1 || option.name.match(regOption)) {
      if (filtered.length < props.menu.maxItem) filtered.push(option);
    }
  }
  return filtered;
});

watch(searchFilter, () => {
  if (filteredOptions.value.length === 0) {
    selected.value = {};
  } else {
    selected.value = filteredOptions.value[0];
  }
  emits('filter', searchFilter.value);
});
</script>

<template>
  <div class="relative block" v-if="props.menu.options">
    <input
      class="bg-white cursor-pointer border border-black rounded block p-2 hover:bg-blue-300 w-[200px]"
      :name="props.menu?.name"
      @focus="showOptions"
      @blur="exit"
      @keyup="keyMonitor"
      v-model="searchFilter"
      :disabled="props.menu.disabled"
      :placeholder="props.menu.placeholder"
    />

    <div
      class="absolute border border-black rounded overflow-auto z-10 bg-white text-black h-40 w-[200px]"
      v-show="optionsShown"
    >
      <div
        class="p-2 block cursor-pointer hover:bg-green-400 min-h-5 w-[190px]"
        @mousedown="selectOption(option)"
        v-for="(option, index) in filteredOptions"
        :key="index"
      >
        {{ option.name || option.id || '-' }}
      </div>
      <p class="p-2 block min-h-5" v-show="filteredOptions.length === 0">Item not found</p>
    </div>
  </div>
</template>
