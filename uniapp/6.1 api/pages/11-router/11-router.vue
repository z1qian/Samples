<template>
	<view class="">
		<view @click="navigateTo">navigateTo</view>
		<view @click="redirectTo">redirectTo</view>
		<view @click="reLaunch">reLaunch</view>
	</view>
</template>

<script setup>
import { ref } from 'vue';

function navigateTo() {
	uni.navigateTo({
		url: '/pages/3-showModal/3-showModal'
	});
}

function redirectTo() {
	uni.redirectTo({
		url: '/pages/3-showModal/3-showModal',
		success(e) {
			console.log('success', e);
		}
	});
}

function reLaunch() {
	uni.reLaunch({
		url: '/pages/index/index',
		success(e) {
			console.log('success', e);
		}
	});
}

console.log(getCurrentPages());
</script>

<style lang="scss" scoped></style>
