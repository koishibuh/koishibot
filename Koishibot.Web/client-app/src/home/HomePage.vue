<script setup lang="ts">
import {computed, nextTick, ref, watch} from 'vue';
import {useAxios} from '@/api/newhttp';
import StreamInfo from '@/home/stream-info/StreamInfo.vue';
import {useStreamInfoStore} from './stream-info/stream-info.store';
import {useSettingsStore} from '@/settings/settings.store';
import {useNotificationStore} from '@/common/notifications/notification.store';

const http = useAxios();
const store = useSettingsStore();
const notificationStore = useNotificationStore();
const streamInfoStore = useStreamInfoStore();
const test = ref();

const goal = ref<{ title: string; amount: number }>({title: '', amount: 0});

// have this check to whatever is received from bot on load
const checked = ref<Boolean>();

async function testFeature() {
  // console.log('Testing 1 2 3');
  // test.value = await http.post('/api/test', null);
  test.value = await http.get('/api/goals/tipjar', null);
}

async function submitGoal() {
  // console.log('Testing 1 2 3');
  await http.post('/api/goals/tipjar', goal.value);
  goal.value = { title: '', amount: 0 };
}

const bannerTest = () => {
  notificationStore.displayMessage('3');
}
const isAmountFocused = ref(false)

const displayAmount = computed(() => {
    if (isAmountFocused.value) {
      // During typing: show raw dollars (without .00)
      return (goal.value.amount / 100).toString()
    } else {
      // On blur: format with .00
      return (goal.value.amount / 100).toFixed(2)
    }
})

const handleAmountInput = (event: Event) => {
  const target = event.target as HTMLInputElement
  const value = target.value

  // Parse raw input as dollars, store as cents
  const num = parseFloat(value.replace(/[^\d.]/g, '')) || 0
  goal.value.amount = Math.round(num * 100)  // Convert to cents
}

const onAmountFocus = async () => {
  isAmountFocused.value = true
  if (goal.value.amount === 0) {
    goal.value.amount = 0
    await nextTick()
    const input = document.getElementById('goalamount') as HTMLInputElement
    input?.select()
  }
}

const onAmountBlur = () => {
  isAmountFocused.value = false
  // Ensure clean cents value
  goal.value.amount = Math.round(goal.value.amount)
}

const isFormValid = computed(() => {
  return goal.value.title.trim() !== '' && goal.value.amount !== 0
})
watch(
    () => checked.value,
    async () => {
      // await http.patch('/api/attendance/status', { ServiceName: 'Attendance', Status: checked.value });
      await http.post('/api/attendance/status', {Status: checked.value});
    }
);
</script>

<template>
  <StreamInfo :info="streamInfoStore.streamInfo"/>

  <h1>Test Zone</h1>
  <div class="flex gap-2 border-2 p-2 border-gray-500 rounded h-24">
    <button @click="testFeature()" class="primary-button">Test Button</button>
    <button @click="bannerTest" class="primary-button">Trigger Banner</button>
  </div>

  <div class="p-2 mb-2 mx-auto max-w-xl">
    <input type="checkbox" id="checkbox" v-model="checked"/>
    <label for="checkbox">Attendance: {{ checked }}</label>
  </div>

  <h1>Stream Summary</h1>
  <div class="p-2 mb-2 w-full border-gray-500 rounded  border-2">
    <textarea v-model="streamInfoStore.streamSummary.summary" class="text-black w-full h-[250px] mb-2"/>
    <div class="flex gap-2">

      <button class="primary-button" @click="() => streamInfoStore.getStreamSummary()">Get Summary</button>
      <button class="primary-button" @click="streamInfoStore.updateStreamSummary()">Update</button>
    </div>
  </div>

  <div>
    <form @submit.prevent="submitGoal()" class="flex flex-col gap-2 my-4">
      <div class="flex flex-col gap-2">
        <label for="title">Goal Title</label>
        <input type="text" v-model="goal.title" id="goaltitle" class="text-black "/>
      </div>
      <div class="flex flex-col gap-2">

        <label for="goalamount">Goal Amount</label>
        <input
            type="text"
            :value="displayAmount"
            @input="handleAmountInput"
            @focus="onAmountFocus"
            @blur="onAmountBlur"
            id="goalamount"
            class="text-black"
            inputmode="decimal"
        />
      </div>
      
      <button class="primary-button" :disabled="!isFormValid">Save</button>
    </form>
  </div>
</template>