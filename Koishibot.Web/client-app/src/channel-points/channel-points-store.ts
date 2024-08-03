import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import { type IChannelPointReward } from './models/channel-point-interface';
import { type IChannelRewardRequest } from './models/reward-request.interface';
import data from './data/channel-point-reward-data.json';
import http from '@/api/http';

export const useChannelPointStore = defineStore('channel-points', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  const channelPointRewards = ref<IChannelPointReward[]>([]);
  /*   const channelPointRewards = ref<IChannelPointReward[]>(data); */

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
      console.log((error as Error).message);
    }
  };

  const editChannelPointReward = async (reward: IChannelPointReward) => {
    try {
      await http.patch('/api/point-rewards/twitch', reward);
    } catch (error) {
      console.log((error as Error).message);
    }
  };

  const deleteChannelPointReward = async (reward: IChannelPointReward) => {
    try {
      await http.delete('/api/point-rewards/twitch', reward);
    } catch (error) {
      console.log((error as Error).message);
    }
  };

  signalRConnection?.on('ReceiveChannelPointReward', (reward: IChannelPointReward) => {
    const index = channelPointRewards.value.findIndex((x) => x.twitchId === reward.twitchId);

    if (index !== 1) {
      channelPointRewards.value.slice(index, 1);
    }

    channelPointRewards.value.push(reward);
  });

  return {
    channelPointRewards,
    getChannelPointRewards,
    createChannelPointReward,
    editChannelPointReward,
    deleteChannelPointReward
  };
});
