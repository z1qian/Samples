<template>
	<view class="">
		<button type="primary" @click="startPullDownRefresh">刷新</button>

		<button type="primary" @click="stopPullDownRefresh">停止刷新</button>
	</view>
</template>

<script setup>
import { ref } from 'vue';
import { onPullDownRefresh } from '@dcloudio/uni-app';
onPullDownRefresh(() => {
	console.log('触发了onPullDownRefresh');
});

function startPullDownRefresh() {
	uni.startPullDownRefresh({
		success(e) {
			console.log('success', e);
		}
	});
}

function stopPullDownRefresh() {
	uni.stopPullDownRefresh();
}
</script>

<style lang="scss" scoped></style>
