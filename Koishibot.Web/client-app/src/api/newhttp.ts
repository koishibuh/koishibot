// For API Calls
import axios from 'axios';
import {useNotificationStore} from '@/common/notifications/notification.store';


export const useAxios = () => {
  const notificationStore = useNotificationStore();

  const get = async <T> (url: string, data: any | null) : Promise<T | null> => {
    try {
      const response = await axios.get(url, { params: data });
      console.log("axios", response);
      return response.data;
    } catch (error) {
      if (axios.isAxiosError(error)) {
        const message = error.response?.data?.status + ': ' + error.response?.data?.title;
        await notificationStore.displayMessageNew(true, message)
      }
      return null;
    }
  }

  const post = async (url: string, data: any | null) => {
    try {
      const response = await axios.post(url, data);
      return response.data;
    } catch (error) {
      if (axios.isAxiosError(error)) {
        const message = error.response?.data?.status + ': ' + error.response?.data?.title + ' - ' + error.response?.data.errors[0];
        await notificationStore.displayMessageNew(true, message);
        throw error;
      }
    }
  };

  const patch = async (url: string, data: any | null, params: any | null) => {
    try {
      const response = await axios.patch(url, data, { params: params });
      return response.data;
    } catch (error) {
      if (axios.isAxiosError(error)) {
        const message = error.response?.data?.status + ': ' + error.response?.data?.title;
        await notificationStore.displayMessageNew(true, message);
      }
    }
  }

  const remove = async (url: string, data: any | null) => {
    try {
      const response = await axios.delete(url, data);
      return response.data;
    } catch (error) {
      if (axios.isAxiosError(error)) {
        const message = error.response?.data?.status + ': ' + error.response?.data?.title;
        await notificationStore.displayMessageNew(true, message);
      }
    }
  }

  return {
    get,
    post,
    patch,
    remove
  };
}

interface ValidationError {
  message: string;
  errors: Record<string, string[]>
}

interface MessageError {
  title: string;
  status: number;
}