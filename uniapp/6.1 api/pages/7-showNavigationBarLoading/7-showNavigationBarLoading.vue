<template>
	<view class="">在当前页面显示导航条加载动画</view>
</template>

<script setup>
import { ref } from 'vue';

uni.showNavigationBarLoading({
	success(e) {
		console.log(e);
	}
});

setTimeout(() => {
	uni.hideNavigationBarLoading();
}, 2000);
</script>

<style lang="scss" scoped></style>
