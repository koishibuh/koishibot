import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import { type IRaidCandidate } from './interfaces/raid-candidate.interface';
import { type IRaidCandidateVm } from './interfaces/raid-candidate.interface';
import { type IRaidPoll } from './interfaces/raid-poll.interface';
import PollDefault from '@/raids/data/pollDefault.json';

export const useRaidStore = defineStore('raidStore', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  const displayOverlay = ref<boolean>(false);

  const multistreamUrl = ref('');
  const raidCandidates = ref<IRaidCandidate[]>([]);

  const raidPoll = ref<IRaidPoll>(PollDefault);

  signalRConnection?.on('ReceiveRaidCandidates', (candidates: IRaidCandidateVm) => {
    console.log(candidates.multistreamUrl);
    console.log(candidates.raidCandidates);
    multistreamUrl.value = candidates.multistreamUrl;
    raidCandidates.value = candidates.raidCandidates;
  });

  signalRConnection?.on('ReceiveRaidOverlayStatus', (overlayStatus: boolean) => {
    displayOverlay.value = overlayStatus;
    console.log('ReceivedRaidOverlayStatus', displayOverlay.value);

    raidPoll.value = {
      currentPollResults: [
        { choice: raidCandidates.value[0].streamerName, voteCount: 0 },
        { choice: raidCandidates.value[1].streamerName, voteCount: 0 },
        { choice: raidCandidates.value[2].streamerName, voteCount: 0 }
      ]
    };

    if (overlayStatus === false) {
      multistreamUrl.value = '';
      raidCandidates.value = [];
      raidPoll.value = PollDefault;
    }
  });

  signalRConnection?.on('ReceiveRaidPollVote', (poll: IRaidPoll) => {
    console.log('ReceiveRaidPollVote', poll);
    raidPoll.value = poll;
  });

  // TODO: When raid is cancelled
  // TODO: Request new raid suggestion to replace streamer who is offline

  return {
    displayOverlay,
    raidCandidates,
    raidPoll
  };
});
