import { ref } from 'vue';
import { defineStore } from 'pinia';
import type { IJwt } from './models/jwt.interface';
import http from '@/api/http';
import { type ITwitchOAuthToken } from './models/twitch-oauth-token.interface';
import type { AxiosResponse } from 'axios';

export const useAuthenticationStore = defineStore('authentication-store', () => {
  const userData = ref<IJwt | null>();

  const loginUser = async (username: string, password: string) => {
    const result = await http.post<AxiosResponse>('/api/login', {
      username: username,
      password: password
    });

    if (result !== null) {
      setUserData(result.data);
    }
  };

  function setUserData(data: IJwt): void {
    userData.value = data;
    localStorage.setItem('user', JSON.stringify(data));
    http.setAuthorizationHeader(data.token);
  }

  function getUserData(): void {
    const data: any = localStorage.getItem('user');
    if (!data) {
      return;
    }
    const parsed = JSON.parse(data);
    console.log(parsed);
    http.setAuthorizationHeader(parsed.token);
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
    getTwitchOAuthToken,
    getUserData
  };
});