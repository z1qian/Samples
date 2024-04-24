<template>
	<view class="classListLayout">
		<view class="loadMoreLayout" v-if="!wallList.length && !noData">
			<uni-load-more status="loading"></uni-load-more>
		</view>

		<view class="content">
			<navigator :url="`/pages/preview/preview?id=${item._id}`" class="item" v-for="item in wallList" :key="item._id">
				<image :src="item.smallPicurl" mode="aspectFill"></image>
			</navigator>
		</view>

		<view class="loadMoreLayout" v-if="wallList.length || noData">
			<uni-load-more :status="noData ? 'noMore' : 'loading'"></uni-load-more>
		</view>

		<view class="safe-area-inset-bottom"></view>
	</view>
</template>

<script setup>
import { ref } from 'vue';
import { apiGetWallList, apiGetUserWallList } from '../../api/apis.js';
import { onLoad, onReachBottom, onShareAppMessage, onShareTimeline, onUnload } from '@dcloudio/uni-app';
import { goHome } from '../../utils/common.js';

const queryParams = {
	pageNum: 1,
	pageSize: 12
};

let noData = false;
let titleName = '';

onLoad((e) => {
	const { id = 0, name = '', type = null } = e;
	// if (!id) {
	// 	goHome();
	// 	return;
	// }

	if (id) {
		queryParams.classid = id;
	}

	if (type) {
		queryParams.type = type;
	}

	titleName = name;

	uni.setNavigationBarTitle({
		title: name
	});

	getWallList();
});

onReachBottom(() => {
	if (!noData) {
		queryParams.pageNum++;
		getWallList();
	} else {
		console.log('noData');
	}
});
const wallList = ref([]);

// 获取分类下的壁纸列表
const getWallList = async () => {
	let res;

	if (queryParams.classid) {
		res = await apiGetWallList(queryParams);
	} else {
		res = await apiGetUserWallList(queryParams);
	}

	wallList.value = [...wallList.value, ...res.data];

	noData = res.data.length < queryParams.pageSize;
	uni.setStorageSync('storgClassList', wallList.value);
	console.log(wallList.value.length);
};

// 发送给朋友
onShareAppMessage((e) => {
	return {
		title: 'zzx壁纸-' + titleName,
		path: `/pages/classlist/classlist?id=${queryParams.classid}&name=${titleName}`
	};
});

// 分享到朋友圈
onShareTimeline(() => {
	// console.log(res);
	return {
		title: '分享到你的朋友圈-' + titleName,
		query: `id=${queryParams.classid}&name=${titleName}`
	};
});

onUnload(() => {
	uni.removeStorageSync('storgClassList');
});
</script>

<style lang="scss" scoped>
.classListLayout {
	.content {
		display: grid;
		grid-template-columns: repeat(3, 1fr);
		gap: 5rpx;
		padding: 5rpx;
		.item {
			height: 440rpx;
			image {
				width: 100%;
				height: 100%;
				display: block;
			}
		}
	}
}
</style>
