import {ref} from 'vue';
import {defineStore} from 'pinia';
import {useSignalR} from '@/api/signalr.composable';
import type {IPoll, IPollChoice} from './models/poll.interface';
import pollVoteData from '@/polls/sample/pollVoteData.json';
import type {IPollChoiceInfo} from "@/raids/models/raid-poll.interface";
import type {IPendingPoll} from "@/polls/models/pending-poll.interface";
import {useNotificationStore} from "@/common/notifications/notification.store";
import http from "@/api/http";
// import {usePollTimerStore} from "@/timers/polltimer.store";

export const usePollStore = defineStore('poll-store', () => {
  const {getConnectionByHub} = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');
  const notificationStore = useNotificationStore();

  const pollDefault = {
    title: '',
    choices: new Array(5).fill(''),
    duration: 300
  };

  const pendingPoll = ref<IPendingPoll>({...pollDefault});

  const enableOverlay = ref<boolean>(false);
  const displayForm = ref<boolean>(true);

  const pollId = ref<string>("");
  const pollTitle = ref<string>("");
  const pollDuration = ref<string>('00:00:00');

  const pollVotes = ref<IPollChoiceInfo[] | null>();
  const voteCountTotal = ref<number>(0);
  const winningChoice = ref<string>('');

  signalRConnection?.on('ReceivePollStarted', (poll: IPoll) => {
    voteCountTotal.value = 0;
    pollId.value = poll.id;
    pollTitle.value = poll.title;
    pollVotes.value = poll.choices;
    pollDuration.value = poll.duration;
    enableOverlay.value = true;

    if (displayForm.value) {
      displayForm.value = false;
    }
  });

  signalRConnection?.on('ReceivePollVote', (votes: IPollChoiceInfo[]) => {
    pollVotes.value = votes;
    voteCountTotal.value = voteCountTotal.value + 1;
  });

  signalRConnection?.on('ReceivePollEnded', (winner: string) => {
    winningChoice.value = winner;

    pollVotes.value = [];
    enableOverlay.value = false;
    voteCountTotal.value = 0;

    if (!displayForm.value) {
      displayForm.value = true;
    }
  });

  const submitPoll = async () => {
    try {
      if (pendingPoll.value.title === '') {
        await notificationStore.displayMessageNew(true, 'Poll title missing');
        return;
      }

      const poll: IPendingPoll = {
        title: pendingPoll.value.title,
        choices: pendingPoll.value.choices.filter(x => x !== ''),
        duration: pendingPoll.value.duration
      };


      if (poll.choices.length < 2) {
        await notificationStore.displayMessageNew(true, 'Need at least 2 choices');
        return;
      }

      await http.post('/api/polls/twitch', poll);
      await notificationStore.displayMessageNew(false, 'Was successful');

      pendingPoll.value = {...pollDefault, choices: new Array(5).fill('')};
    } catch (error) {
      await notificationStore.displayMessage((error as Error).message);
    }
  }

  const cancelPoll = async () => {
    try {
      await http.delete('/api/polls/twitch');
      await notificationStore.delay(1);
      displayForm.value = true;
    } catch (error) {
      await notificationStore.displayMessage((error as Error).message);
    }
  }

  return {
    pendingPoll,
    pollTitle,
    pollVotes,
    pollDuration,
    voteCountTotal,
    winningChoice,
    displayForm,
    enableOverlay,
    submitPoll,
    cancelPoll
  };
});