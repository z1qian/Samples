<template>
	<view class="container">
		<view class="menu">
			<uni-segmented-control :current="current" :values="controlTextAry" style-type="button" active-color="#2B9939" @clickItem="onClickItem" />
		</view>

		<view class="layout">
			<view class="box" v-for="(item, index) in pets" :key="item._id">
				<view class="pic">
					<image lazy-load :src="item.url" mode="widthFix" @click="onPreview(index)"></image>
				</view>

				<view class="text">{{ item.content }}</view>
				<view class="author">from 【{{ item.author }}】</view>
			</view>
		</view>

		<view class="float">
			<view class="item" @click="onRefresh">
				<uni-icons type="refreshempty" size="26" color="#888"></uni-icons>
			</view>
			<view class="item" @click="onTop">
				<uni-icons type="arrow-up" size="26" color="#888"></uni-icons>
			</view>
		</view>

		<view class="loadMore">
			<uni-load-more status="loading"></uni-load-more>
		</view>
	</view>
</template>

<script setup>
import { computed, ref } from 'vue';
import { onReachBottom, onPullDownRefresh } from '@dcloudio/uni-app';

const pets = ref([]);
const controlData = [
	{ key: 'all', value: '所有' },
	{ key: 'cat', value: '猫猫' },
	{ key: 'dog', value: '狗狗' }
];

const controlTextAry = computed(() => controlData.map((c) => c.value));
const current = ref(0);

function getImages() {
	uni.showNavigationBarLoading();
	uni.request({
		url: 'https://tea.qingnian8.com/tools/petShow',
		data: {
			size: 2,
			type: controlData[current.value].key
		},
		header: {
			'access-key': 'qwer123'
		}
	})
		.then((res) => {
			const { data } = res;

			if (data.errCode === 0) {
				pets.value = [...pets.value, ...data.data];
			} else {
				uni.showToast({
					title: data.errMsg,
					icon: 'none',
					duration: 3000
				});
			}
		})
		.catch((err) => {
			uni.showToast({
				title: '服务器异常，请稍后重试',
				icon: 'none'
			});
		})
		.finally(() => {
			uni.hideNavigationBarLoading();
			uni.stopPullDownRefresh();
		});
}

getImages();

// 点击预览
const onPreview = (index) => {
	let urls = pets.value.map((item) => item.url);
	uni.previewImage({
		current: index,
		urls
	});
};

//触底加载更多
onReachBottom(() => {
	getImages();
});

//下拉刷新
onPullDownRefresh(() => {
	pets.value = [];
	current.value = 0;
	getImages();
});

//刷新
function onRefresh() {
	console.log('onRefresh');
	uni.startPullDownRefresh();
}

//顶部
function onTop() {
	uni.pageScrollTo({
		scrollTop: 0,
		duration: 100
	});
}

//点击菜单
function onClickItem(e) {
	current.value = e.currentIndex;
	pets.value = [];
	getImages();
}
</script>

<style lang="scss" scoped>
.container {
	.menu {
		padding: 50rpx 50rpx 0;
	}
	.layout {
		padding: 50rpx;
		.box {
			margin-bottom: 60rpx;
			box-shadow: 0 30rpx 40rpx rgba(0, 0, 0, 0.08);
			border-radius: 15rpx;
			overflow: hidden;
			.pic {
				image {
					width: 100%;
				}
			}
			.text {
				padding: 30rpx;
				color: #333;
				font-size: 36rpx;
			}

			.author {
				padding: 0 30rpx 30rpx;
				text-align: right;
				color: #888;
				font-size: 28rpx;
			}
		}
	}

	.float {
		position: fixed;
		right: 30rpx;
		bottom: 80rpx;
		padding-bottom: env(safe-area-inset-bottom);
		.item {
			width: 90rpx;
			height: 90rpx;
			background-color: rgba(255, 255, 255, 0.9);
			border-radius: 50%;
			margin-bottom: 20rpx;
			display: flex;
			justify-content: center; /* 水平居中 */
			align-items: center; /* 垂直居中 */
			border: 1px solid #eee;
		}
	}

	.loadMore {
		padding-bottom: calc(env(safe-area-inset-bottom) + 50rpx);
	}
}
</style>
