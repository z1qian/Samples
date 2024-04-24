<template>
	<view class="">
		<!-- <life-child></life-child> -->
		<scroll-view scroll-y="true" scroll-x ref="sc"></scroll-view>
		<life-child v-if="show"></life-child>
	</view>
</template>

<script setup>
import { onMounted, ref } from 'vue';
const sc = ref(null);

onMounted(() => {
	console.log(sc.value);
});

const show = ref(true);

// setInterval(() => {
// 	show.value = !show.value;
// }, 3000);
</script>

<style lang="scss" scoped></style>