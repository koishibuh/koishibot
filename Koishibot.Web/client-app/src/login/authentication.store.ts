import { ref } from 'vue';
import { defineStore } from 'pinia';
import type { IJwt } from '@/login/jwt.interface';
import http from '@/api/http';
import { type ITwitchOAuthToken } from './models/twitch-oauth-token.interface';

export const useAuthenticationStore = defineStore('authentication-store', () => {
  const userData = ref<IJwt | null>();

  const loginUser = async (username: string, password: string) => {
    const result = await http.post<IJwt>('/api/login', { username: username, password: password });
    if (result !== null) {
      setUserData(result);
    }
  };

  /*   async function loginUser(username: string, password: string) {
    const result = await http.post('/api/login', { username: username, password: password });
    setUserData(result);
  } */

  function setUserData(data: IJwt): void {
    userData.value = data;
    localStorage.setItem('user', JSON.stringify(data));
    http.setAuthorizationHeader(data.token);
  }

  const refreshTwitchOAuthToken = async (code: string): Promise<ITwitchOAuthToken> => {
    return await http.post('/api/twitch-auth/', { token: code });
  };

  const getTwitchOAuthToken = async (code: string) => {
    return await http.post('/api/twitch-auth/token', { code: code });
  };

  return {
    loginUser,
    refreshTwitchOAuthToken,
    getTwitchOAuthToken
  };
});
