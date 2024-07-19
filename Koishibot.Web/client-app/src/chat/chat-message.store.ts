import { ref } from 'vue';
import { defineStore } from 'pinia';
import { type IChatMessage } from './chat-message.interface';
import { useSignalR } from '@/api/signalr.composable';
import chatSample from '@/chat/data/chatSample.json';

export const useChatMessageStore = defineStore('chat-messages', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  /*   const chatMessages = ref<IChatMessage[]>([]); */
  const chatMessages = ref<IChatMessage[]>(chatSample);

  signalRConnection?.on('ReceiveChatMessage', (chatmessage: IChatMessage) => {
    console.log('ReceiveChatMessage', chatmessage);
    if (chatMessages.value.length >= 50) {
      chatMessages.value.splice(0, 1);
    }
    chatMessages.value.push(chatmessage);
  });

  return {
    chatMessages
  };
});
