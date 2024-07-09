<template>
	<view class="homeLayout pageBg">
		<custom-nav-bar title="推荐"></custom-nav-bar>
		<!-- banner -->
		<view class="banner">
			<swiper indicator-dots indicator-color="rgba(255, 255, 255,.5)" indicator-active-color="rgb(255,255,255)" autoplay interval="3000" circular>
				<swiper-item v-for="item in bannerList" :key="item._id">
					<navigator 
					class="link" 
					:url="`/pages/classlist/classlist?${item.url}`"
					v-if="item.target=='self'">
						<image :src="item.picurl" mode="aspectFill"></image>
					</navigator>
					
					<navigator
					class='link' 
					:url="item.url"
					target='miniProgram'
					:app-id="item.appid"
					v-else>
						<image :src="item.picurl" mode="aspectFill"></image>
					</navigator>
				</swiper-item>
			</swiper>
		</view>
		<!-- notice -->
		<view class="notice">
			<view class="left">
				<uni-icons type="sound-filled" size="28rpx"></uni-icons>
				<text class="text">公告</text>
			</view>
			<view class="center">
				<swiper autoplay circular vertical interval="1500" duration="300">
					<swiper-item v-for="(item, index) in noticeList" :key="item._id">
						<navigator :url="`/pages/notice/detail?id=${item._id}`">{{ item.title }}</navigator>
					</swiper-item>
				</swiper>
			</view>
			<view class="right">
				<uni-icons type="right" size="25rpx" color="#333"></uni-icons>
			</view>
		</view>
		<!-- select -->
		<view class="select">
			<common-title>
				<template #name>每日推荐</template>
				<template #custom>
					<view class="date">
						<uni-icons type="calendar" size="40rpx"></uni-icons>
						<uni-dateformat :date="Date.now()" format="dd日" class="day"></uni-dateformat>
					</view>
				</template>
			</common-title>
			<view class="content">
				<scroll-view scroll-x>
					<view class="box" v-for="item in dailyRecommendList" :key="item._id" @click="goPreview(item._id)">
						<image :src="item.smallPicurl" mode="aspectFill"></image>
					</view>
				</scroll-view>
			</view>
		</view>
		<!-- theme -->
		<view class="theme">
			<common-title>
				<template #name>专题精选</template>
				<template #custom>
					<navigator 
						class="more" 
						url="/pages/classify/classify"
						open-type="reLaunch"
					>
						More+
					</navigator>
				</template>
			</common-title>
			<view class="content">
				<theme-item v-for="item in classifyList" :key="item._id" :item="item"></theme-item>
				<theme-item :isMore="true"></theme-item>
			</view>
		</view>
	</view>
</template>

<script setup>
import { ref } from 'vue';
import { apiGetBanner, apiGetDailyRecommend, apiGetNoticeList, apiGetClassifyList } from '../../api/apis.js';
import { onShareAppMessage, onShareTimeline } from '@dcloudio/uni-app';

const bannerList = ref([]);
const dailyRecommendList = ref([]);
const noticeList = ref([]);
const classifyList = ref([]);

// 预览壁纸
function goPreview(id) {
	uni.setStorageSync('storgClassList', dailyRecommendList.value);
	uni.navigateTo({
		url: '/pages/preview/preview?id=' + id
	});
}

// 获取轮播图
const getBanner = async () => {
	const res = await apiGetBanner();
	bannerList.value = res.data;
};

// 获取每日推荐
const getDailyRecommend = async () => {
	const res = await apiGetDailyRecommend();
	dailyRecommendList.value = res.data;
};

// 获取公告
const getNoticeList = async () => {
	const res = await apiGetNoticeList({ select: true });
	noticeList.value = res.data;
};

// 获取专题精选
const getClassifyList = async () => {
	const res = await apiGetClassifyList({ select: true });
	classifyList.value = res.data;
};

// 发送给朋友
onShareAppMessage((e) => {
	return {
		title: 'zzx壁纸',
		path: '/pages/index/index'
	};
});

// 分享到朋友圈
onShareTimeline(() => {
	// console.log(res);
	return {
		title: '分享到你的朋友圈',
		imageUrl: '/static/logo.png'
	};
});

getBanner();
getDailyRecommend();
getNoticeList();
getClassifyList();
</script>

<style lang="scss" scoped>
.homeLayout {
	.banner {
		width: 750rpx;
		padding: 30rpx 0;
		
		swiper {
			width: 100%;
			height: 340rpx;
			
			&-item {
				width: 100%;
				height: 100%;
				padding: 0 30rpx;
				.link{
					width: 100%;
					height: 100%;
					image {
						width: 100%;
						height: 100%;
						border-radius: 10rpx;
					}
				}	
			}
		}
	}

	.notice {
		width: 690rpx;
		height: 80rpx;
		line-height: 80rpx;
		display: flex;
		background: #f9f9f9;
		border-radius: 80rpx;
		margin: 0 auto;

		.left {
			width: 140rpx;
			display: flex;
			align-items: center;
			justify-content: center;
			:deep() {
				.uniui-sound-filled {
					color: $brand-theme-color !important;
				}
			}
			.text {
				color: $brand-theme-color;
				font-weight: 600;
				font-size: 28rpx;
			}
		}
		.center {
			flex: 1;
			swiper {
				width: 100%;
				height: 100%;
				&-item {
					font-size: 30rpx;
					color: #666;
					overflow: hidden;
					white-space: nowrap;
					text-overflow: ellipsis;
				}
			}
		}

		.right {
			width: 70rpx;
			display: flex;
			align-items: center;
			justify-content: center;
		}
	}

	.select {
		margin-top: 50rpx;
		.date {
			display: flex;
			align-items: center;
			color: $brand-theme-color;
			:deep() {
				.uniui-calendar {
					color: $brand-theme-color !important;
				}
			}

			.day {
				margin-left: 5rpx;
			}
		}
		.content {
			width: 720rpx;
			margin-left: 30rpx;
			margin-top: 30rpx;
			scroll-view {
				white-space: nowrap;
				.box {
					width: 200rpx;
					height: 430rpx;
					display: inline-block;
					margin-right: 15rpx;
					image {
						width: 100%;
						height: 100%;
						border-radius: 10rpx;
					}
				}
				.box:last-child {
					margin-right: 30rpx;
				}
			}
		}
	}

	.theme {
		padding: 50rpx 0;
		.more {
			color: #888;
		}

		.content {
			padding: 30rpx 30rpx 0;
			display: grid;
			gap: 15rpx;
			grid-template-columns: repeat(3, 1fr);
		}
	}
}
</style>
