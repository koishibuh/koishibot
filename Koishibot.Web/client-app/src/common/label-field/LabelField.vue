<script setup lang="ts">
import {ref, watch, computed, onMounted} from "vue";
import { usePollStore} from "@/polls/poll-store";

const store = usePollStore();

const props = defineProps<{
  label: string;
  labelId: string;
  textLimit: number;
  text: string;
}>()

const textField = ref('');

onMounted(() => {
  textField.value = props.text;
})

watch(() => props.text,
    () => {
  textField.value = props.text;
});

const textColor = computed(() => {
  const percentage = (textField.value.length / props.textLimit) * 100;
  switch (true) {
    case percentage == 100:
      return 'text-red-500';
    case percentage > 80:
      return 'text-orange-500';
    case percentage > 50:
      return 'text-yellow-500';
    default:
      return 'text-white';
  }
});

// const emit = defineEmits<{
//   (e: 'update', payload: { name: string, text: string }): any;
// }>();

// const sendValue = () => {
//   emit('update', {name: props.labelDescription, text: textField.value});
// }

const updateValue = () => {
  switch (props.labelId) {
    case 'pollTitle':
      return store.pendingPoll.title = textField.value;
    case 'choiceOne':
      return store.pendingPoll.choices[0] = textField.value;
    case 'choiceTwo':
      return store.pendingPoll.choices[1] = textField.value;
    case 'choiceThree':
      return store.pendingPoll.choices[2] = textField.value;
    case 'choiceFour':
      return store.pendingPoll.choices[3] = textField.value;
    case 'choiceFive':
      return store.pendingPoll.choices[4] = textField.value;
    default:
      return;
  }
}
</script>

<template>
  <div class="flex flex-col">
    <div class="flex justify-between">
      <label for="labelId">{{ label }}</label>
      <p :class="textColor">{{ textField.length }} / {{ textLimit }}</p>
    </div>
    <input type="text" v-model="textField" id="labelId" class="text-black"
           :maxlength="textLimit"
           @blur="updateValue">
  </div>
</template>