import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import { useNotificationStore } from '@/common/notifications/notification.store';
import type { IChannelPointReward, IChannelRewardRequest } from './models/channel-point.interface';
import http from '@/api/http';
import data from './data/channelRewardData.json';

export const useChannelPointStore = defineStore('channel-points', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');
  const notificationStore = useNotificationStore();

  const channelPointRewards = ref<IChannelPointReward[]>([]);
  /*   const channelPointRewards = ref<IChannelPointReward[]>(data); */

  signalRConnection?.on('ReceiveChannelPointReward', (reward: IChannelPointReward) => {
    const index = channelPointRewards.value.findIndex((x) => x.twitchId === reward.twitchId);

    if (index !== 1) {
      channelPointRewards.value.slice(index, 1);
    }

    channelPointRewards.value.push(reward);
  });

  const getChannelPointRewards = async () => {
    if (channelPointRewards.value.length === 0) {
      const response = await http.get<IChannelPointReward[]>('/api/point-rewards');
      channelPointRewards.value.push(...response);
    }
  };

  const createChannelPointReward = async (reward: IChannelRewardRequest) => {
    try {
      await http.post('/api/point-rewards/twitch', reward);
    } catch (error) {
      notificationStore.displayMessage((error as Error).message);
    }
  };

  const editChannelPointReward = async (reward: IChannelPointReward) => {
    try {
      await http.patch('/api/point-rewards/twitch', reward);
    } catch (error) {
      notificationStore.displayMessage((error as Error).message);
    }
  };

  const deleteChannelPointReward = async (reward: IChannelPointReward) => {
    try {
      await http.delete('/api/point-rewards/twitch', reward);
    } catch (error) {
      notificationStore.displayMessage((error as Error).message);
    }
  };

  return {
    channelPointRewards,
    getChannelPointRewards,
    createChannelPointReward,
    editChannelPointReward,
    deleteChannelPointReward
  };
});
