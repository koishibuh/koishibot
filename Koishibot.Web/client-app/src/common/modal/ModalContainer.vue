<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import ObsSettings from '@/settings/ObsSettings.vue';
import ScopesSettings from '@/settings/ScopesSettings.vue';

const components: { [key: string]: any } = {
  ObsSettings: ObsSettings,
  ScopesSettings: ScopesSettings
};

const props = defineProps<{
  content: string;
  /* display: boolean; */
}>();

const getContentComponent = computed(() => {
  /*  if (props.content === 'obs') {
    return ObsSettings;
  } else {
    return ScopesSettings;
  } */
  return components[props.content];
});

const showModal = ref<boolean>(true);

const emit = defineEmits<{
  modalClosed: [boolean];
}>();

const closeModal = () => {
  emit('modalClosed', false);
};

const formattedHeader = computed(() => {
  return props.content.replace(/(?<!^)([A-Z])/g, ' $1');
});
</script>

<template>
  <transition name="faded" appear>
    <div class="modal-overlay" @click="closeModal"></div>
  </transition>

  <transition name="slide" appear>
    <div
      class="modal fixed w-[550px] border-2 border-gray-500 bg-[#3f4046] rounded"
      v-if="showModal"
    >
      <div class="flex justify-between bg-background p-2 rounded-t">
        <h1>{{ formattedHeader }}</h1>
        <button class="primary-button w-[30px]" @click="closeModal">x</button>
      </div>
      <div class="max-h-56 overflow-y-auto">
        <component :is="getContentComponent" @saveClicked="closeModal" class="p-2" />
      </div>
    </div>
  </transition>
</template>

<style scoped>
.modal-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 98;
  background-color: rgba(0, 0, 0, 0.646);
}

.modal {
  z-index: 99;
}

.faded-enter-active,
.faded-leave-active {
  transition: opacity 0.5s ease;
}

.faded-enter,
.faded-leave-to {
  opacity: 0;
}

.slide-enter-active,
.slide-leave-active {
  transition: transform 0.5s;
}

.slide-enter,
.slide-leave-to {
  transform: translateY(-50%) translateX(100vw);
}
</style>