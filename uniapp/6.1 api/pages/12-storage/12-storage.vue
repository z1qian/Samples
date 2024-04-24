<template>
	<view class="">
		<button @click="remove">移除指定缓存</button>
		<button @click="clear" type="warn">清除所有缓存</button>
	</view>
</template>

<script setup>
import { ref } from 'vue';

uni.setStorage({
	key: 'name',
	data: '李四',
	success: function () {
		console.log('setStorage', 'success');
	}
});

uni.getStorage({
	key: 'name',
	success: function (res) {
		console.log(res);
		console.log(res.data);
	},
	fail(res) {
		console.log(res);
	},
	complete(res) {
		console.log(res);
	}
});

uni.getStorageInfo({
	success: function (res) {
		console.log(res);
		console.log(res.keys);
		console.log(res.currentSize);
		console.log(res.limitSize);
	}
});

function remove() {
	uni.removeStorage({
		key: 'name',
		success: function (res) {
			console.log(res);
		}
	});
}

function clear() {
	uni.clearStorage();
}
</script>

<style lang="scss" scoped></style>
