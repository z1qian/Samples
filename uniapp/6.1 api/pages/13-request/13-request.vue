<template>
	<view class="layout">
		<view class="row" v-for="item in requestData" :key="item.id">
			<view class="title">{{ item.title }}</view>
			<view class="content">{{ item.body }}</view>
		</view>
	</view>
</template>

<script setup>
import { ref } from 'vue';

const requestData = ref([]);

uni.showLoading({
	title: '加载中'
});
uni.request({
	url: 'https://jsonplaceholder.typicode.com/posts',
	success(res) {
		console.log('success');
		requestData.value = res.data;
	},
	timeout: 2000,
	fail(err) {
		console.log('超时');
		console.log(err);
	},
	complete() {
		uni.hideLoading();
	}
});

// uni.request({
// 	url: 'https://jsonplaceholder.typicode.com/posts'
// }).then((res) => {
// 	console.log(res);
// 	requestData.value = res.data;
// });
// async function getPosts() {
// 	let res = await uni.request({
// 		url: 'https://jsonplaceholder.typicode.com/posts',
// 		data: {
// 			id: 1
// 		},
// 		header: {
// 			token: '123token'
// 			// 'Content-Type': 'text/plain'
// 		},
// 		// method: 'POST',
// 		timeout: 1000,
// 		dataType: 'json',

// 	});
// 	console.log(res);
// 	requestData.value = res.data;
// }

// getPosts();
</script>

<style lang="scss" scoped>
.layout {
	padding: 30rpx;
	.row {
		border-bottom: 1px solid #cfcfcf;
		padding: 20rpx 0;
		.title {
			font-size: 36rpx;
			margin-bottom: 10rpx;
		}
		.content {
			font-size: 28rpx;
			color: #666;
		}
	}
}
</style>
