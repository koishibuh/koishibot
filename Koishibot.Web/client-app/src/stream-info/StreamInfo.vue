<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useStreamInfoStore } from './stream-info.store';

const store = useStreamInfoStore();
const streamtitle = ref<string>();
const category = ref<string>();

onMounted(async () => {
  try {
    await store.GetStreamInfo();
    streamtitle.value = store.streamInfo?.streamTitle;
    category.value = store.streamInfo?.category;
  } catch (error) {
    console.log(error);
  }
});

async function updateStreamInfo() {
  await store.UpdateStreamInfo(streamtitle.value, category.value);
}
</script>

<template>
  <div class="">
    <h1>Stream Info</h1>
    <div class="border-2 p-2 border-gray-500 rounded flex items-center gap-2">
      <div class="w-1/3">
        <button class="primary-button mb-2">Previous Info</button>
        <button class="primary-button mb-2">Coding</button>
        <button class="primary-button">Art</button>
      </div>
      <div class="w-2/3">
        <form @submit.prevent="updateStreamInfo()" class="flex flex-col gap-2 my-4">
          <label for="streamtitle">Title</label>
          <input type="text" v-model="streamtitle" id="streamtitle" class="text-black" />
          <label for="category">Category</label>
          <input type="text" v-model="category" id="category" class="text-black" />
          <button class="primary-button">Update</button>
        </form>
      </div>
    </div>
  </div>
</template>
