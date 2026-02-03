// import { ref } from 'vue';
// import { defineStore } from 'pinia';
// import { useSignalR } from '@/api/signalr.composable';
// import http from "@/api/http.ts";
//
// export const useGoalStore = defineStore('goal', () => {
//     const { getConnectionByHub } = useSignalR();
//     const signalRConnection = getConnectionByHub('notifications');
//     //
//     // const goals = ref<IGoal[]>([]);
//     // const bitGoal = ref<IGoal>({goalType: 'Bits', currentAmount: 0, goalAmount: 500});
//     // const subGoal = ref<IGoal>({goalType: 'Subs', currentAmount: 0, goalAmount: 5});
//     // /*   const streamEvents = ref<IStreamEvent[]>(eventSample); */
//     //
//     // signalRConnection?.on('ReceiveGoalEvent', (goalEvent: IGoalEvent) => {
//     //     if (goalEvent.goalType === 'Bits') {
//     //         bitGoal.value.currentAmount = bitGoal.value?.currentAmount + goalEvent.amount;
//     //     } else if (goalEvent.goalType === 'Subs') {
//     //         subGoal.value.currentAmount = subGoal.value?.currentAmount + goalEvent.amount;
//     //     }
//     //     // streamEvents.value.push(streamEvent);
//     // });
//     //
//     // const getBitGoal = async () => {
//     //     const result: IGoalEvent = await http.get('/api/goals/bits');
//     //     if (result)
//     //     {
//     //         bitGoal.value = {goalType: 'Bits', currentAmount: result.amount, goalAmount: 500};
//     //     }
//     // };
//     //
//     // const getSubGoal = async () => {
//     //     const result: IGoalEvent = await http.get('/api/goals/subs');
//     //     if (result)
//     //     {
//     //         subGoal.value = {goalType: 'Subs', currentAmount: result.amount, goalAmount: 5};
//     //     }
//     // };
//
//     return {
//         bitGoal,
//         subGoal,
//         getBitGoal,
//         getSubGoal
//     };
// });
//
// // export interface IGoal {
// //     goalType: string;
// //     currentAmount: number;
// //     goalAmount: number;
// // }
// //
// // export interface IGoalEvent {
// //     goalType: string;
// //     amount: number;
// // }