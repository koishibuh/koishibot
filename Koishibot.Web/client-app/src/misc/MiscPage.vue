<script setup lang="ts">
import {ref, watch} from "vue";
import {useAxios} from "@/api/newhttp";
import {useNotificationStore} from "@/common/notifications/notification.store";
import {type IDragonEgg} from "@/misc/models/dragon-eggs.interface";
import dragonEggData from "@/misc/data/dragonEggData.json";

const http = useAxios();
const notifications = useNotificationStore();

const eggDescription = ref<IDragonEgg | null>();

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
  eggDescription.value = null;
}

const getEggs = async (location: string) => {
  const url = `https://dragcave.net/locations/${location}`;
  window.open(url);
  const result: IDragonEgg = await http.post('/api/dragon-quest/page', {location: url});
  eggDescription.value = result;
}

const username = ref<string>('');

const createUserItemTag = async () => {
  try {
    await http.post('/api/wordpress/item-tag', {username: username.value.toLowerCase()});
  } catch (error) {
    console.log('Unable to add item tag for user');
  }
}
</script>

<template>
  <div class="border-2 p-2 border-gray-500 rounded flex-col items-center gap-2">
    <div v-if="eggDescription">
      <div v-for="(item, index) in eggDescription" :key="index">
        <p>Egg {{ index }} - {{ item }}</p>
      </div>
    </div>
    <div class="flex gap-2">
      <button class="primary-button w-full" @click="getEggs('5-alpine')">Alpine</button>
      <button class="primary-button w-full" @click="getEggs('1-coast')">Coast</button>
      <button class="primary-button w-full" @click="getEggs('2-desert')">Desert</button>
      <button class="primary-button w-full" @click="getEggs('3-forest')">Forest</button>
      <button class="primary-button w-full" @click="getEggs('4-jungle')">Jungle</button>
      <button class="primary-button w-full" @click="getEggs('6-volcano')">Volcano</button>
    </div>
    <form @submit.prevent="saveDragon()" class="flex flex-col gap-2 my-4">
      <label for="code">Dragon Code</label>
      <input type="text" v-model="code" id="code" class="text-black"/>
      <button class="primary-button">Save</button>
    </form>
  </div>

  <div>
    <form @submit.prevent="createUserItemTag()" class="flex flex-col gap-2 my-4">
      <label for="username">Twitch Username</label>
      <input type="text" v-model="username" id="username" class="text-black"/>
      <button class="primary-button">Save</button>
    </form>
  </div>
</template>