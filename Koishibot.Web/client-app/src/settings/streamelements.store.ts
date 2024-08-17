import { ref, computed } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import http from '@/api/http';
import { useNotificationStore } from '@/common/notifications/notification.store';

export const useStreamElementsStore = defineStore('streamElementsStore', () => {
    const notificationStore = useNotificationStore();

    const saveJwtToken = async (token: string) => {
      try {
        await http.post('api/stream-elements/token', { jwtToken: token });
      } catch {
         await notificationStore.displayMessage('Error with saving token');
      }
    };

    const connectStreamElements = async () => {
        try{
            await http.post('api/stream-elements', null);
        }
        catch {
            await notificationStore.displayMessage('Jwt token has expired');
        }
    }

    return {
        saveJwtToken,
        connectStreamElements
    };
});