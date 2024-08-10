import './common/assets/main.css';

import { createApp } from 'vue';
import { createPinia } from 'pinia';

import App from './App.vue';
import router from './layout/router/router-index';
import { useSignalR } from './api/signalr.composable';

const app = createApp(App);
const { createSignalRConnection } = useSignalR();

createSignalRConnection('notifications').then(() => {
  app.use(createPinia());
  app.use(router);
  app.mount('#app');
});
