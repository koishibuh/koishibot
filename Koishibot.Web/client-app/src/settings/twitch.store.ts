import { ref, computed } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import http from '@/api/http';
import { useNotificationStore } from '@/common/notifications/notification.store';
import { type ILog } from '@/settings/models/log-interface';
import type {AxiosError} from "axios";

export const useTwitchStore = defineStore('twitchStore', () => {
  const notificationStore = useNotificationStore();

  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  const ircStatus = ref({ name: 'BotIrc', status: false });
  const eventSubStatus = ref({ name: 'TwitchWebsocket', status: false });

  const connectToTwitch = async () => {
    try {
      await notificationStore.displayMessage('Getting Authorization Link');
      return await http.get<string>('/api/twitch-auth/url');
    } catch (error) {
      await notificationStore.displayMessage((error as Error).message);
    }
  }

  const reconnectTwitchServices = async () => {
    try {
      await notificationStore.displayMessage('Starting Twitch Services');
      return await http.post<string>('/api/stream/reconnect', null);
    } catch (error) {
      await notificationStore.displayMessage("Stream is offline");
    }
  };

  const updateIrcStatus = async (enabled: boolean) => {
    try {
      if (enabled) {
        await http.post('/api/twitch-irc', null);
        await notificationStore.displayMessage('Twitch Irc Connected');
      } else {
        await http.delete('/api/twitch-irc');
        await notificationStore.displayMessage('Twitch Irc Disconnected');
      }
    } catch (error) {
      await notificationStore.displayMessage((error as Error).message);
    }
  };

  const updateEventSubStatus = async (enabled: boolean) => {
    try {
      if (enabled) {
        await http.post('/api/twitch-eventsub', null);
        await notificationStore.displayMessage('Twitch EventSub Connected');
      } else {
        await http.delete('/api/twitch-eventsub');
        await notificationStore.displayMessage('Twitch EventSub Disconnected');
      }
    } catch (error) {
      await notificationStore.displayMessage((error as Error).message);
    }
  };

  return {
    ircStatus,
    eventSubStatus,
    connectToTwitch,
    reconnectTwitchServices,
    updateIrcStatus,
    updateEventSubStatus
  };
});