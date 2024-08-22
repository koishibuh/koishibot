<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useStreamInfoStore } from './stream-info.store';
import type { IStreamInfo, IStreamInfoRequest } from './models/stream-info.interface';

const store = useStreamInfoStore();

const props = defineProps<{
  info: IStreamInfo;
}>();

const streamTitle = ref<string>(props.info.streamTitle);
const category = ref<string>(props.info.category);
const categoryId = ref<string>(props.info.categoryId);

/* onMounted(async () => {
   await store.getStreamInfo();
  streamtitle.value = store.streamInfo?.streamTitle!;
  category.value = store.streamInfo?.category!;
}); */

const getStreamInfo = async () => {
  await store.getStreamInfo();
  streamTitle.value = store.streamInfo?.streamTitle!;
  category.value = store.streamInfo?.category!;
  categoryId.value = store.streamInfo?.categoryId!;
};

const updateStreamInfo = async () => {
  const request: IStreamInfoRequest = {
    streamTitle: streamTitle?.value,
    categoryId: categoryId?.value
  };

  await store.updateStreamInfo(request);
};
</script>

<template>
  <div>
    <h1>Stream Info</h1>
    <div class="border-2 p-2 border-gray-500 rounded flex items-center gap-2">
      <div class="w-1/3 flex flex-col gap-2">
        <button class="primary-button">Previous Info</button>
        <button class="primary-button">Coding</button>
        <button class="primary-button">Art</button>
        <button class="primary-button" @click="getStreamInfo">Refresh</button>
      </div>
      <div class="w-2/3">
        <form @submit.prevent="updateStreamInfo()" class="flex flex-col gap-2 my-4">
          <label for="streamtitle">Title</label>
          <input type="text" v-model="streamTitle" id="streamtitle" class="text-black" />
          <label for="category">Category</label>
          <input type="text" v-model="category" id="category" class="text-black" />
          <button class="primary-button">Update</button>
        </form>
      </div>
    </div>
  </div>
</template>