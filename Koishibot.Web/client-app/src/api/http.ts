// For API Calls
import axios, {AxiosError} from 'axios';

export default {
  async get<T>(url: string): Promise<T> {
    const response = await axios.get(url);
    return response.data;
  },

  async post<T>(url: string, data: any | null): Promise<T> {
    return await axios.post(url, data);
  },

  async patch<T>(url: string, data?: any | null): Promise<T> {
    return await axios.patch(url, data);
  },

  async delete<T>(url: string, data?: any | null): Promise<T> {
    return await axios.delete(url, data);
  },

  setAuthorizationHeader(token: string): void {
    axios.interceptors.request.use((config) => {
      config.headers.Authorization = `Bearer ${token}`;
      return config;
    });
  }
};