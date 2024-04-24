<template>
	<view class="">
		<button type="primary" @click="onClick">隐藏返回首页按钮</button>
	</view>
</template>

<script setup>
import { ref } from 'vue';

function onClick() {
	uni.hideHomeButton({
		success(e) {
			console.log('success', e);
		}
	});
}
</script>

<style lang="scss" scoped></style>
