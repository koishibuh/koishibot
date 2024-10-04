<script setup lang="ts">
import { watch, ref, onMounted } from 'vue';
import { useRaidStore } from '@/raids/raid.store';

const store = useRaidStore();

</script>

<template>
 <div class="" v-if="store.displayShoutoutOverlay">
   <p class="text-white text-3xl">OVERLAY</p>
   <a :href="store.raidShoutoutUrl"></a>
 </div>
</template>