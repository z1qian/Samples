<template>
	<view class="">
		<button type="primary" @click="show">显示</button>
		<button type="default" @click="hide">隐藏</button>
		<navigator url="/pages/user/user" class="nav" open-type="reLaunch">跳转到用户界面</navigator>
	</view>
</template>

<script setup>
import { ref } from 'vue';

function show() {
	uni.showToast({
		title: 'success',
		image: '../../static/logo.png',
		mask: false,
		duration: 3000,
		success: (e) => {
			console.log('success', e);
		},
		fail: (e) => {
			console.log('fail', e);
		},
		complete: (e) => {
			console.log('complete', e);
		}
	});
}

function hide() {
	uni.hideToast();
}
</script>

<style lang="scss" scoped>
.nav {
	background-color: yellow;
}
</style>
