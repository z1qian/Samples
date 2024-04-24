<template>
	<view class="">
		<button type="primary" @click="onClick">动态设置当前页面的标题</button>
	</view>
</template>

<script setup>
import { ref } from 'vue';

function onClick() {
	uni.setNavigationBarTitle({
		title: '动态设置页面标题',
		success(e) {
			console.log(e);
		}
	});
}
</script>

<style lang="scss" scoped></style>
