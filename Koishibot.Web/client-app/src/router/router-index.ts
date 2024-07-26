import { createRouter, createWebHistory } from 'vue-router';
import PollPage from '@/polls/PollPage.vue';
import HomePage from '@/home/HomePage.vue';
import FeedPage from '@/event-feed/FeedPage.vue';
import CommandPage from '@/commands/CommandPage.vue';
import RaidOverlay from '@/raids/RaidOverlay.vue';
import TimerOverlay from '@/timers/TimerOverlay.vue';
import BotPage from '@/layout/BotPage.vue';
import CalendarOverlay from '@/calendars/CalendarOverlay.vue';
import PollOverlay from '@/polls/PollOverlay.vue';
import LoginPage from '@/login/LoginPage.vue';
import AdTimerOverlay from '@/ad-timer/AdTimerOverlay.vue';
import DandleOverlay from '@/dandle/DandleOverlay.vue';
import SettingsPage from '@/settings/SettingsPage.vue';
import LightsPage from '@/lights/LightsPage.vue';
import ChannelPointPage from '@/channel-points/ChannelPointPage.vue';
import DandlePage from '@/dandle/DandlePage.vue';
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
          component: PollPage
        },
        {
          path: '/feed',
          name: 'Feed',
          component: FeedPage
        },
        {
          path: '/command',
          name: 'Command',
          component: CommandPage
        },
        {
          path: '/lights',
          name: 'Lights',
          component: LightsPage
        },
        {
          path: '/settings',
          name: 'Settings',
          component: SettingsPage
        },
        {
          path: '/channelpoints',
          name: 'Channel Points',
          component: ChannelPointPage
        },
        {
          path: '/commands',
          name: 'Commands',
          component: CommandPage
        },
        {
          path: '/dandle',
          name: 'Dandle',
          component: DandlePage
        }
      ]
    },
    {
      path: '/overlay/raid',
      name: 'RaidOverlay',
      component: RaidOverlay
    },
    {
      path: '/overlay/timer',
      name: 'TimerOverlay',
      component: TimerOverlay
    },
    {
      path: '/overlay/adtimer',
      name: 'AdTimerOverlay',
      component: AdTimerOverlay
    },
    {
      path: '/overlay/calendar',
      name: 'CalendarOverlay',
      component: CalendarOverlay
    },
    {
      path: '/overlay/poll',
      name: 'PollOverlay',
      component: PollOverlay
    },
    {
      path: '/overlay/dandle',
      name: 'DandleOverlay',
      component: DandleOverlay
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
