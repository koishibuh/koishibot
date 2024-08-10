<script setup lang="ts">
import { watch, ref } from 'vue';
import { useRaidStore } from '@/raids/raid.store';
import { type IRaidCandidate } from './models/raid-candidate.interface';

const store = useRaidStore();
const streamLoading = ref<boolean>(true);

function createVideoUrl(username: string): string {
  return `https://player.twitch.tv/?channel=${username}&muted=true&parent=localhost&player=popout`;
}

const showOne = ref(true);

watch(
  () => store.displayOverlay,
  async () => {
    if (store.displayOverlay === true) {
      await sleep(20000).then(() => (streamLoading.value = false));
      console.log('watch', streamLoading.value);
    } else {
      streamLoading.value = true;
    }
  }
);

const sleep = (ms: number) => {
  return new Promise((resolve, reject) => setTimeout(resolve, ms));
};

function getData(streamerName: string): IRaidCandidate | null {
  const suggestion = store.raidCandidates?.filter((x) => x.streamerName === streamerName);
  if (suggestion === undefined) {
    return null;
  } else {
    return suggestion[0];
  }
}
</script>

<template>
  <div class="flex flex-col h-screen">
    <div class="grow"></div>
    <div v-if="store.displayOverlay" class="flex justify-between px-4 mb-2">
      <div
        v-for="item in store.raidPoll.currentPollResults"
        :key="item.choice"
        class="mt-10 mx-2 flex flex-col relative w-[600px]"
      >
        <div class="z-10 absolute w-full">
          <div class="border-2 border-black rounded bg-white mr-10 ml-20 h-8 flex">
            <div class="flex justify-center z-20 relative left-[-25px] top-[-5px] p-4">
              <Transition name="fadename">
                <div v-if="showOne" class="absolute text-3xl font-bold top-[-3px]">
                  {{ item.voteCount }}
                </div>
              </Transition>
            </div>
            <img
              src="./images/20240624-twitchoverlay-raid-flower.png"
              class="flower h-[65px] absolute left-10 top-[-20px]"
            />
            <p class="self-center ml-1 mb-1 text-2xl font-bold">{{ item.choice }}</p>
          </div>
        </div>

        <div
          v-show="streamLoading"
          class="bg-blue-500 h-[334px] w-[593px] mx-2 mt-4 border-2 rounded-xl overflow-hidden text-2xl absolute p-8"
        >
          <div>Loading {{ getData(item.choice)?.streamerName }}'s stream.</div>
          <div>Thanks to {{ getData(item.choice)?.suggestedByName }} for the suggestion!</div>
        </div>

        <div class="bg-blue-500 h-[334px] w-[593px] mx-2 mt-4 border-2 rounded-xl overflow-hidden">
          <iframe
            :src="createVideoUrl(item.choice)"
            frameborder="0"
            allowfullscreen="false"
            scrolling="no"
            width="593"
            height="334"
          ></iframe>
        </div>
      </div>
    </div>
  </div>
</template>

<style>
.fadename-enter-from {
  opacity: 0;
  transform: translateY(6px);
}

.fadename-enter-to {
  opacity: 1;
  transform: translateY(0);
}

.fadename-enter-active {
  transition: all 800ms ease;
}

.fadename-leave-from {
  opacity: 1;
  transform: translateY(0px);
}

.fadename-leave-to {
  opacity: 0;
  transform: translateY(-6px);
}

.fadename-leave-active {
  transition: all 800ms ease;
}

.flower {
  animation: spinanimation 10s ease-in-out infinite;
}

@keyframes spinanimation {
  0% {
    transform: rotate(360deg);
  }
}
</style>
