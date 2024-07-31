import { ref } from 'vue';
import { defineStore } from 'pinia';
import http from '@/api/http';
import { type ICommand } from './command-interface';

export const useCommandStore = defineStore('command-store', () => {
  const commands = ref<ICommand[]>();

  const createCommandName = async (name: string) => {
    return await http.post('/api/commands/names', { name: name });
  };

  return {
    createCommandName
  };
});
