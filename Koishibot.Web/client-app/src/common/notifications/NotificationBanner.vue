<script setup lang="ts">
defineProps<{
  message: string;
  error: boolean;
}>();

const backgroundColor = (error: boolean): string => {
  if (error) {
    return 'bg-red-500';
  } else {
    return 'bg-green-400';
  }
}
</script>

<template>
  <Transition name="slide">
    <div
      class="z-10 absolute left-[25%] top-[35px] m-auto w-2/4 p-2 rounded"
      :class="backgroundColor(error)"
      v-if="message !== ''"
    >
      <p>{{ message }}</p>
    </div>
  </Transition>
</template>

<style scoped>
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