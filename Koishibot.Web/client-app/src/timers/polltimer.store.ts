// import {computed, ref, watch} from 'vue';
// import {defineStore} from 'pinia';
// import type {ITimeSpan} from "@/timers/models/timespan.interface";
//
// export const usePollTimerStore = defineStore('pollTimerStore', () => {
//
//   const pollTimer = ref<ITimeSpan>({duration: "00:10:00"});
//
//   const currentHour = ref(0);
//   const currentMinute = ref(0);
//   const currentSecond = ref(0);
//
//   let intervalId: number = 1000;
//
//   watch(pollTimer, () => {
//     const [hours, minutes, seconds] = pollTimer.value.duration.split(":").map(Number);
//     currentHour.value = hours;
//     currentMinute.value = minutes;
//     currentSecond.value = seconds;
//
//     startTimer();
//   });
//
//   const formattedMinutes = computed(() => {
//     return currentMinute.value < 1 ? '00' : currentMinute.value.toString().padStart(2, '0');
//   });
//
//   const formattedSeconds = computed(() => {
//     return currentSecond.value.toString().padStart(2, '0')
//   })
//
//   const startTimer = function () {
//     if (intervalId) clearInterval(intervalId);
//
//     intervalId = setInterval(() => {
//       if (currentSecond.value === 0) {
//         {
//           console.log("0")
//         }
//         if (currentMinute.value === 0) {
//           clearInterval(intervalId);
//         } else {
//           currentMinute.value--;
//           currentSecond.value = 59;
//         }
//       } else {
//         currentSecond.value--;
//       }
//     }, 1000);
//   };
//
//   return {
//     currentSecond,
//     pollTimer
//   };
// });