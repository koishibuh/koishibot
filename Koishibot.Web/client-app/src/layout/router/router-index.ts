import { createRouter, createWebHistory } from 'vue-router';
import HomePage from '@/home/HomePage.vue';
import FeedPage from '@/event-feed/FeedPage.vue';
import BotPage from '@/layout/BotPage.vue';
import LoginPage from '@/login/LoginPage.vue';
import AuthenticatePage from '@/login/AuthenticatePage.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/bot',
      name: 'Bot',
      meta: { requiresAuth: true },
      component: BotPage,
      children: [
        {
          path: '',
          name: 'Home',
          component: HomePage
        },
        {
          path: '/authenticate',
          name: 'authenticate',
          component: AuthenticatePage
        },
        {
          path: '/poll',
          name: 'Poll',
          component: () => import('@/polls/PollPage.vue')
        },
        {
          path: '/feed',
          name: 'Feed',
          component: FeedPage
        },
        {
          path: '/commands',
          name: 'Commands',
          component: () => import('@/commands/CommandPage.vue')
        },
        {
          path: '/lights',
          name: 'Lights',
          component: () => import('@/lights/LightsPage.vue')
        },
        {
          path: '/settings',
          name: 'Settings',
          component: () => import('@/settings/SettingsPage.vue')
        },
        {
          path: '/channelpoints',
          name: 'Channel Points',
          component: () => import('@/channel-points/ChannelPointPage.vue')
        },
        {
          path: '/dandle',
          name: 'Dandle',
          component: () => import('@/dandle/DandlePage.vue')
        },
        {
          path: '/log',
          name: 'Log',
          component: () => import('@/console-log/LogPage.vue')
        }
      ]
    },
    {
      path: '/overlay/raid',
      name: 'RaidOverlay',
      component: () => import('@/raids/RaidOverlay.vue')
    },
    {
      path: '/overlay/timer',
      name: 'TimerOverlay',
      component: () => import('@/timers/TimerOverlay.vue')
    },
    {
      path: '/overlay/adtimer',
      name: 'AdTimerOverlay',
      component: () => import('@/timers/AdTimerOverlay.vue')
    },
    {
      path: '/overlay/calendar',
      name: 'CalendarOverlay',
      component: () => import('@/calendars/CalendarOverlay.vue')
    },
    {
      path: '/overlay/poll',
      name: 'PollOverlay',
      component: () => import('@/polls/PollOverlay.vue')
    },
    {
      path: '/overlay/dandle',
      name: 'DandleOverlay',
      component: () => import('@/dandle/DandleOverlay.vue')
    },
    {
      path: '/',
      name: 'Login',
      component: LoginPage
    }
  ]
});

router.beforeEach((to, from, next) => {
  const loggedIn = localStorage.getItem('user');

  if (to.matched.some((record) => record.meta.requiresAuth) && !loggedIn) {
    next('/');
  } else {
    next();
  }
});

export default router;