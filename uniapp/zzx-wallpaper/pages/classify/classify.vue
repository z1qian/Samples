<template>
	<view class="classifyLayout pageBg">
		<custom-nav-bar title="分类"></custom-nav-bar>
		<view class="classify">
			<theme-item v-for="item in classifyList" :key="item._id" :item="item"></theme-item>
		</view>
	</view>
</template>

<script setup>
import { ref } from 'vue';
import { apiGetClassifyList } from '../../api/apis.js';
import { onShareAppMessage, onShareTimeline } from '@dcloudio/uni-app';

const classifyList = ref([]);

// 获取专题精选
const getClassifyList = async () => {
	const res = await apiGetClassifyList({ pageSize: 15 });
	classifyList.value = res.data;
};

// 发送给朋友
onShareAppMessage((e) => {
	return {
		title: 'zzx壁纸',
		path: '/pages/classify/classify'
	};
});

// 分享到朋友圈
onShareTimeline(() => {
	// console.log(res);
	return {
		title: '分享到你的朋友圈'
	};
});

getClassifyList();
</script>

<style lang="scss" scoped>
.classifyLayout {
	.classify {
		padding: 30rpx;
		display: grid;
		gap: 15rpx;
		grid-template-columns: repeat(3, 1fr);
	}
}
</style>
