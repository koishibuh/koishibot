import {ref} from 'vue';
import {defineStore} from 'pinia';
import http from '@/api/http';
import type {
  ICommand,
  ICommandName,
  IChatCommandInfo,
  ICommandRequest
} from './models/command-interface';
import type {IDropdownMenu} from '@/common/dropdown-menu/dropdownmenu-interface';
import {useNotificationStore} from '@/common/notifications/notification.store';

export const useCommandStore = defineStore('command-store', () => {
  const notificationStore = useNotificationStore();

  const commands = ref<ICommand[] | null>();
  const availableNames = ref<ICommandName[] | null>();

  const availableNamesOptions = ref<IDropdownMenu>({
    name: 'commandNames',
    options: [],
    placeholder: 'Select Command Name',
    disabled: false,
    maxItem: 1000
  });

  const createCommandName = async (name: string) => {
    try {
      return await http.post('/api/commands/name', {name: name});
    } catch (error) {
      await notificationStore.displayErrorMessage((error as Error).message);
    }
  };

  const getCommands = async () => {
    try {
      if (commands.value == null) {
        console.log('test2');
        const result = await http.get<IChatCommandInfo>('/api/commands/');
        commands.value = result.commands;
        availableNames.value = result.commandNames;
        availableNamesOptions.value.options.push(...result.commandNames);
      }
    } catch (error) {
      await notificationStore.displayMessage((error as Error).message);
    }
  };

  const createCommand = async (request: ICommandRequest) => {
    try {
      const result: number = await http.post('/api/commands', {request: request});

      const index = availableNamesOptions.value.options.findIndex(
        (option) => option.id === request.commandNames[0].id
      );
      if (index !== -1) {
        availableNamesOptions.value.options.splice(index, 1);
      }

      const newCommand: ICommand = {
        id: result,
        category: request.category,
        description: request.description,
        enabled: request.enabled,
        message: request.message,
        globalCooldown: request.globalCooldownMinutes,
        userCooldown: request.userCooldownMinutes,
        permissions: request.permissionLevel,
        names: [request.commandNames[0]]
      };

      commands.value?.push(newCommand);
    } catch (error) {
      notificationStore.displayMessage((error as Error).message);
    }
  };

  return {
    commands,
    availableNames,
    availableNamesOptions,
    createCommandName,
    getCommands,
    createCommand
  };
});