import { ref } from 'vue';
import { defineStore } from 'pinia';
import { type IChatMessage } from './models/chat-message.interface';
import { useSignalR } from '@/api/signalr.composable';
import http from '@/api/http';
import { useNotificationStore } from '@/common/notifications/notification.store';
import data from '@/chat/data/chatData.json';

export const useChatMessageStore = defineStore('chat-messages', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');
  const notificationStore = useNotificationStore();

  /*   const chatMessages = ref<IChatMessage[]>([]); */
  const chatMessages = ref<IChatMessage[]>(data);

  signalRConnection?.on('ReceiveChatMessage', (chatmessage: IChatMessage) => {
    console.log('ReceiveChatMessage', chatmessage);
    if (chatMessages.value.length >= 50) {
      chatMessages.value.splice(0, 1);
    }
    chatMessages.value.push(chatmessage);
  });

  const sendChatMessage = async (message: string) => {
    try {
      await http.post('/api/chat', { message: message });
    } catch (error) {
      notificationStore.displayMessage((error as Error).message);
    }
  };

  return {
    chatMessages,
    sendChatMessage
  };
});
