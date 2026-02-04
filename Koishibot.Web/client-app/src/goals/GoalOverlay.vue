<script setup lang="ts">
import { storeToRefs } from 'pinia'
import {computed, onMounted, ref} from "vue";
import {useEventFeedStore} from "@/event-feed/event-feed.store.ts";
import GoalBar from "@/common/goal-bar/GoalBar.vue";

const store = useEventFeedStore();

onMounted(async () => {
  try {
    {
      await store.getOverlayGoals();
    }
  } catch (error) {
    console.log(error);
  }
})

const goals = storeToRefs(store);
</script>

<template>
  <GoalBar :goal="goals.tipJarGoal" :addDecimal="true"/>
  <GoalBar :goal="goals.subGoal" :addDecimal="false"/>
</template>