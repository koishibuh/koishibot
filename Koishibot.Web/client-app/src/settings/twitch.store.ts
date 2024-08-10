import { ref, computed } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import http from '@/api/http';
import { useNotificationStore } from '@/common/notifications/notification.store';
import { type ILog } from '@/settings/models/log-interface';

export const useTwitchStore = defineStore('twitchStore', () => {
  const notificationStore = useNotificationStore();

  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  const ircStatus = ref<boolean>(false);
  const eventSubStatus = ref<boolean>(false);

  const updateIrcStatus = async (enabled: boolean) => {
    try {
      if (enabled) {
        await http.post('/api/twitch-irc', null);
        notificationStore.displayMessage('Twitch Irc Connected');
      } else {
        await http.delete('/api/twitch-irc');
        notificationStore.displayMessage('Twitch Irc Disconnected');
      }
    } catch (error) {
      notificationStore.displayMessage((error as Error).message);
    }
  };

  const updateEventSubStatus = async (enabled: boolean) => {
    try {
      if (enabled) {
        await http.post('/api/twitch-eventsub', null);
        notificationStore.displayMessage('Twitch EventSub Connected');
      } else {
        await http.delete('/api/twitch-eventsub');
        notificationStore.displayMessage('Twitch EventSub Disconnected');
      }
    } catch (error) {
      notificationStore.displayMessage((error as Error).message);
    }
  };

  return {
    ircStatus,
    eventSubStatus,
    updateIrcStatus,
    updateEventSubStatus
  };
});
