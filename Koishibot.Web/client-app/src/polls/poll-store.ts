import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import type { IPoll, IPollChoice } from './models/poll.interface';

export const usePollStore = defineStore('poll-store', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  const choices: IPollChoice = { choice1: 1, choice2: 4, choice3: 10 };
  const testPoll: IPoll = {
    id: '0',
    title: 'A poll title1',
    startedAt: new Date(2024, 5, 1),
    endingAt: new Date(2024, 5, 2),
    duration: '0',
    choices: choices
  };

  const currentPollDuration = ref<string>('');

  const currentPoll = ref<IPoll | null>(testPoll);

  signalRConnection?.on('ReceivePoll', (poll: IPoll) => {
    if (currentPoll.value !== null && poll.id === currentPoll.value.id) {
      currentPoll.value == poll;
    } else {
      currentPoll.value = poll;
      currentPollDuration.value = poll.duration;
      console.log(currentPoll.value.duration);
    }
  });

  return {
    currentPoll,
    currentPollDuration
  };
});
