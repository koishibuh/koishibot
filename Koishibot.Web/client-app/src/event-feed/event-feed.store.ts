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
  const recentEvent = ref();

  signalRConnection?.on('ReceiveStreamEvent', (streamEvent: IStreamEvent) => {
    streamEvents.value.push(streamEvent);
    recentEvent.value = streamEvent;
  });

  const getRecentEvents = async () => {
    streamEvents.value = await http.get('/api/event-feed');
    recentEvent.value = streamEvents.value[0];
  };

  return {
    streamEvents,
    recentEvent,
    getRecentEvents
  };
});
