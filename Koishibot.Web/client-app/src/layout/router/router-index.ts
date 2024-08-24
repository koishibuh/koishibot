import { createRouter, createWebHistory } from 'vue-router';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/bot',
      name: 'Bot',
      meta: { requiresAuth: true },
      component: () => import('@/layout/BotPage.vue'),
      children: [
        {
          path: '',
          name: 'Home',
          component: () => import('@/home/HomePage.vue')
        },
        {
          path: '/authenticate',
          name: 'authenticate',
          component: () => import('@/login/AuthenticatePage.vue')
        },
        {
          path: '/attendance',
          name: 'Attendance',
          component: () => import('@/attendance/AttendancePage.vue')
        },
        {
          path: '/poll',
          name: 'Poll',
          component: () => import('@/polls/PollPage.vue')
        },
        {
          path: '/feed',
          name: 'Feed',
          component: () => import('@/event-feed/FeedPage.vue')
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
      component: () => import('@/login/LoginPage.vue')
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