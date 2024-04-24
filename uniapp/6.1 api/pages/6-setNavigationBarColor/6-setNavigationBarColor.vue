<template>
	<view class=""></view>
</template>

<script setup>
import { ref } from 'vue';

uni.setNavigationBarColor({
	frontColor: '#ffffff',
	backgroundColor: '#000080',
	animation: {
		duration: 3000,
		timingFunc: 'easeOut'
	},
	success(e) {
		console.log('success', e);
	}
});
</script>

<style lang="scss" scoped></style>
