<script setup lang="ts">
import {ref, watch} from "vue";
import {useAxios} from "@/api/newhttp";
import {useNotificationStore} from "@/common/notifications/notification.store";

const http = useAxios();
const notifications = useNotificationStore();

const code = ref<string>('');
const saveDragon = async () => {
  if (code.value.length === 0) {
    await notifications.displayMessageNew(true, "Cannot submit blank value");
    return;
  }

  if (code.value.length > 5) {
    code.value = code.value.replace("https://dragcave.net/view/", "");
  }

  await http.post('/api/dragon-quest/wordpress', {code: code.value});
  code.value = '';
}
</script>

<template>
  <div class="border-2 p-2 border-gray-500 rounded flex-col items-center gap-2">
    <form @submit.prevent="saveDragon()" class="flex flex-col gap-2 my-4">
      <label for="username">Dragon Code</label>
      <input type="text" v-model="code" id="code" class="text-black"/>
      <button class="primary-button">Save</button>
    </form>
  </div>
</template>