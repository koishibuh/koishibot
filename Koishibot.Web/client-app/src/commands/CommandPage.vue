<script setup lang="ts">
import { ref, onMounted } from 'vue';
import http from '@/api/http';
import { useCommandStore } from './command.store';
import DropdownMenu from '@/common/dropdown-menu/DropdownMenu.vue';
import type { ICommandName, ICommandRequest } from './command-interface';

const commandName = ref('');

const message = ref<unknown | string>();

const store = useCommandStore();

const nametext = ref('');
const selectedName = ref<ICommandName | null>(null);
const messagetext = ref('');
const permissionstext = ref('');
const cooldowntext = ref('');
const globalcooldowntext = ref('');
const descriptiontext = ref('');
const shouldBeDisabled = ref<boolean>(false);

async function sendCommand() {
  try {
    shouldBeDisabled.value = true;
    if (selectedName.value === null) {
      return;
    }
    const list: ICommandName[] = [selectedName.value];

    const request: ICommandRequest = {
      commandNames: list,
      description: descriptiontext.value,
      enabled: true,
      message: messagetext.value,
      permissionLevel: permissionstext.value,
      userCooldownMinutes: parseInt(cooldowntext.value),
      globalCooldownMinutes: parseInt(globalcooldowntext.value),
      timerGroupIds: null
    };

    await store.createCommand(request);
    selectedName.value = null;
    descriptiontext.value = '';
    messagetext.value = '';
    permissionstext.value = '';
    cooldowntext.value = '';
    globalcooldowntext.value = '';
    shouldBeDisabled.value = false;
  } catch (error) {
    message.value = (error as Error).message;
  }
}

async function createCommandName() {
  try {
    await store.createCommandName(commandName.value.toLowerCase());
    commandName.value = '';
  } catch (error) {
    message.value = (error as Error).message;
  }
}

async function optionSelected(selection: any) {
  if (!selection) {
    return;
  } else {
    selectedName.value = selection;
  }
}

async function getDropdownValues(keyword: any) {
  console.log(keyword);
}

onMounted(() => {
  store.getCommands();
});
</script>

<template>
  {{ message }}
  <h1>Create Command Name</h1>
  <div class="border-2 p-2 border-gray-500 rounded">
    <form @submit.prevent="createCommandName" class="flex flex-col gap-2 my-4">
      <label for="commandname">Command Name:</label>
      <input type="text" v-model="commandName" id="commandname" class="text-black" />
      <button class="primary-button">Send</button>
    </form>
  </div>

  <h1>Current Commands</h1>
  <div class="border-2 p-2 border-gray-500 rounded">
    <div v-for="(item, index) in store.commands" :key="index" class="border mb-2 rounded">
      <div class="flex gap-2">
        <div class="border-r p-2 w-1/6">
          <div v-for="(name, index) in item.names" :key="index">
            {{ name.name }}
          </div>
        </div>
        <div class="w-5/6 p-2">
          {{ item.message }}
        </div>
        <div>
          {{ item.permissions }}
        </div>
      </div>
    </div>
  </div>

  <h1>Test Zone</h1>
  <div class="border-2 p-2 border-gray-500 rounded">
    <form @submit.prevent="sendCommand()" class="flex flex-col gap-2 my-4">
      <!--  <label for="name">Name:</label>
      <input type="text" v-model="nametext" id="name" class="text-black" />
 -->
      <label for="alias">Alias:</label>
      <DropdownMenu
        :menu="store.availableNamesOptions"
        @optionSelected="optionSelected"
        @filter="getDropdownValues"
        disabled="false"
      />
      <label for="description">Description:</label>
      <input type="text" v-model="descriptiontext" id="message" class="text-black" />
      <label for="message">Message:</label>
      <input type="text" v-model="messagetext" id="message" class="text-black" />
      <label for="permissions">Permissions:</label>
      <input type="text" v-model="permissionstext" id="permissions" class="text-black" />
      <label for="cooldown">Cooldown In Minutes:</label>
      <input type="text" v-model="cooldowntext" id="cooldown" class="text-black" />
      <label for="cooldown">Global Cooldown in Minutes:</label>
      <input type="text" v-model="globalcooldowntext" id="cooldown" class="text-black" />
      <button class="primary-button" :disabled="shouldBeDisabled">Send</button>
    </form>
  </div>
</template>
