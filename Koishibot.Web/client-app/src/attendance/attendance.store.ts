import {ref} from 'vue';
import {defineStore} from 'pinia';
import {useSignalR} from '@/api/signalr.composable';
import {useAxios} from "@/api/newhttp";


export const useAttendanceStore = defineStore('attendanceStore', () => {
  const {getConnectionByHub} = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');
  const http = useAxios();

  const currentUserInfo = ref<ITwitchUser>();

  signalRConnection?.on('ReceiveAttendance', () => {
  });

  const getTwitchUserInfo = async (username: string) => {
    const data = {username: username};
    const response = await http.get<ITwitchUser>('/api/twitch-user/twitch', data)
    if (response) {
      currentUserInfo.value = response;
    }
  }

  const saveTwitchUser = async () => {
    const data = currentUserInfo.value;
    await http.post('/api/twitch-user', data);
  }

  return {
    currentUserInfo,
    getTwitchUserInfo,
    saveTwitchUser
  };
});

export interface ITwitchUser {
  id: string;
  login: string;
  name: string;
}