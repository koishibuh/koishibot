import { ref } from 'vue';
import { defineStore } from 'pinia';
import http from '@/api/http';
import type {
  ICommand,
  ICommandName,
  IChatCommandInfo,
  ICommandRequest
} from './command-interface';
import type { IDropdownMenu } from '@/common/dropdown-menu/dropdownmenu-interface';

export const useCommandStore = defineStore('command-store', () => {
  const commands = ref<ICommand[] | null>();
  const availableNames = ref<ICommandName[] | null>();
  const message = ref('');

  const availableNamesOptions = ref<IDropdownMenu>({
    name: 'commandNames',
    options: [],
    placeholder: 'Select Command Name',
    disabled: false,
    maxItem: 1000
  });

  const createCommandName = async (name: string) => {
    return await http.post('/api/commands/names', { name: name });
  };

  const getCommands = async () => {
    console.log('test');
    try {
      if (commands.value == null) {
        console.log('test2');
        const result = await http.get<IChatCommandInfo>('/api/commands/');
        commands.value = result.commands;
        availableNames.value = result.commandNames;
        availableNamesOptions.value.options.push(...result.commandNames);
      }
    } catch (error) {
      message.value = (error as Error).message;
    }
  };

  const createCommand = async (request: ICommandRequest) => {
    try {
      const result: number = await http.post('/api/commands', { request: request });

      console.log('searching for commandName', request.commandNames[0].id);
      console.log('commandName length', request.commandNames.length);

      const index = availableNamesOptions.value.options.findIndex(
        (option) => option.id === request.commandNames[0].id
      );
      if (index !== -1) {
        availableNamesOptions.value.options.splice(index, 1);
      }

      console.log('currently in availablenameoptions', availableNamesOptions.value.options);
      console.log('count availablenameoptions', availableNamesOptions.value.options.length);

      const newCommand: ICommand = {
        id: result,
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
      message.value = (error as Error).message;
    }
  };

  return {
    createCommandName,
    getCommands,
    createCommand,
    commands,
    availableNames,
    availableNamesOptions
  };
});
