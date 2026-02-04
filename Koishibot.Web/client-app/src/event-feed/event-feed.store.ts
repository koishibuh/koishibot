import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import { type IStreamEvent } from './models/stream-event.interface';
import http from '@/api/http';
import eventSample from '@/event-feed/data/eventData.json';

export const useEventFeedStore = defineStore('event-feed', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  const streamEvents = ref<IStreamEvent[]>([]);
  /*   const streamEvents = ref<IStreamEvent[]>(eventSample); */

  const goals = ref<IGoal[]>([]);
  const tipJarGoal = ref<IGoal>({goalType: 'TipJar', currentAmount: 0, goalAmount: 5000, title: "Bitty Tip Jar Goal: Unknown"});
  const subGoal = ref<IGoal>({goalType: 'Subs', currentAmount: 0, goalAmount: 5, title: "Daily Sub Goal"});
  /*   const streamEvents = ref<IStreamEvent[]>(eventSample); */
  
  signalRConnection?.on('ReceiveStreamEvent', (streamEvent: IStreamEvent) => {
    streamEvents.value.push(streamEvent);
    
    if (streamEvent.eventType === 'Cheer' && streamEvent.amount) {
      tipJarGoal.value.currentAmount = tipJarGoal.value?.currentAmount + streamEvent.amount;
    } else if (streamEvent.eventType === 'Kofi' && streamEvent.amount) {
      tipJarGoal.value.currentAmount = tipJarGoal.value?.currentAmount + streamEvent.amount;
    } else if (streamEvent.eventType === 'Sub') {
      subGoal.value.currentAmount += 1;
    }
  });

  const getRecentEvents = async () => {
    streamEvents.value = await http.get('/api/event-feed');
  };
  
  const getOverlayGoals = async () => {
    const result: IGoalVm = await http.get('/api/goals');
    subGoal.value.currentAmount = result.subGoal.currentAmount;
    
    if (result.tipJar === null)
    {
      tipJarGoal.value.currentAmount = 0;
    } else {
      tipJarGoal.value.currentAmount = result.tipJar.currentAmount;
      tipJarGoal.value.goalAmount = result.tipJar.goalAmount;
      tipJarGoal.value.title = 'Bitty TipJar Goal: ' + result.tipJar.title;
    }
  }
  
  
  return {
    streamEvents,
    getRecentEvents,
    tipJarGoal,
    subGoal,
    getOverlayGoals
  };
});

export interface IGoal {
  goalType: string;
  currentAmount: number;
  goalAmount: number;
  title: string | null;
}

export interface IGoalVm {
  tipJar: ITipJar | null;
  subGoal: ISubGoal;
}

export interface ITipJar {
  title: string;
  currentAmount: number;
  goalAmount: number;
}

export interface ISubGoal{
  currentAmount: number;
}