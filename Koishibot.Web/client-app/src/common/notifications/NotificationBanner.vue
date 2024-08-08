<script setup lang="ts">
defineProps<{
  message: string;
}>();
</script>

<template>
  <Transition name="slide">
    <div
      class="z-10 absolute bg-green-400 left-[25%] top-[35px] m-auto w-2/4 p-2 rounded"
      v-if="message !== ''"
    >
      <p>{{ message }}</p>
    </div>
  </Transition>
</template>

<style>
.slide-enter-active {
  transition:
    transform 0.5s,
    opacity 0.5s ease;
}
.slide-leave-active {
  transition: all 0.8s cubic-bezier(1, 0.5, 0.8, 1);
}

.slide-enter,
.slide-leave-to {
  transform: translateY(-5%);
  opacity: 0;
}

.slide-enter-to,
.slide-leave {
  transform: translateY(5%);
}
</style>
