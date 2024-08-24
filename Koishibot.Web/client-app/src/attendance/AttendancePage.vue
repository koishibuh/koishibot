<script setup lang="ts">
import {ref, watch} from "vue";
import {type ITwitchUser, useAttendanceStore} from "@/attendance/attendance.store";

const store = useAttendanceStore();

const currentUser = ref<ITwitchUser>();
const username = ref<string>('');

const getTwitchUserInfo = async () => {
  if (username.value) {
  await store.getTwitchUserInfo(username.value);
  username.value = '';
  }
};

watch(() => store.currentUserInfo, (newValue) => {
  currentUser.value = newValue
})
</script>

<template>
<!-- TODO:-->
<!--  Enable/Disable Attendance-->
<!--  Calendar-->
<!--  Add Users-->

  <div class="border-2 p-2 border-gray-500 rounded flex-col items-center gap-2">
    ID for {{ username }} {{ currentUser?.name }}: {{ currentUser?.id }}
    <form @submit.prevent="getTwitchUserInfo()" class="flex flex-col gap-2 my-4">
        <label for="username">Username</label>
        <input type="text" v-model="username" id="username" class="text-black" />
        <button class="primary-button">Search</button>
      </form>
    <button class="primary-button w-full" @click="store.saveTwitchUser()">Save</button>
  </div>

</template>