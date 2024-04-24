<template>
	<view class="">
		<button @click="showModal" type="primary">显示模态弹窗</button>
		<button @click="navigateBack" type="default">返回上一页面或多级页面</button>
	</view>
</template>

<script setup>
import { ref } from 'vue';

function showModal() {
	uni.showModal({
		title: '提示的标题',
		// content: '提示的内容',
		showCancel: true,
		cancelText: 'No',
		cancelColor: '#008b8b',
		confirmText: 'Yes',
		confirmColor: '#191970',
		editable: true,
		placeholderText: '输入框的提示文本',
		success: (e) => {
			console.log('success', e);
			console.log('success', e.content);
		},
		fail(e) {
			console.log('fail', e);
		},
		complete(e) {
			console.log('complete', e);
			console.log('complete', e.content);
		}
	});
}

console.log(getCurrentPages());

function navigateBack() {
	uni.navigateBack({
		delta: 1
	});
}
</script>

<style lang="scss" scoped></style>
