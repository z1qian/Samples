<template>
	<view class="">
		<button type="primary" @click="onClick">从底部向上弹出操作菜单</button>
	</view>
</template>

<script setup>
import { ref } from 'vue';

let ary = ['A', 'B', 'C', 'D', 'E', 'F'];
function onClick() {
	uni.showActionSheet({
		title: '菜单的标题',
		alertText: '警示的文案（同菜单标题）',
		itemList: ary,
		itemColor: '#cd5c5c',
		success: function (res) {
			// console.log('success', res);
			console.log('success', ary[res.tapIndex]);
		},
		fail: function (res) {
			console.log('fail', res.errMsg);
		},
		complete(e) {
			console.log('complete', e);
		}
	});
}
</script>

<style lang="scss" scoped></style>
