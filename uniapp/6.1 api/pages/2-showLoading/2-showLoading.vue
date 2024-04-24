<template>
	<view class="">
		<button @click="onClick" type="primary">点击我</button>
	</view>
</template>

<script setup>
import { ref } from 'vue';

uni.showLoading({
	title: '加载中...',
	mask: true,
	success(e) {
		console.log('success', e);
	},
	fail(e) {
		console.log('fail', e);
	},
	complete(e) {
		console.log('complete', e);
	}
});

setTimeout(function () {
	uni.hideLoading();
}, 2000);

function onClick() {
	console.log('onClick');
}
</script>

<style lang="scss" scoped></style>
