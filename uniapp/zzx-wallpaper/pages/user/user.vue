<template>
	<view class="userLayout pageBg">
		<view :style="{ height: getNavBarHeight() + 'px' }"></view>

		<view class="userInfo">
			<image src="../../static/logo.png"></image>

			<view class="ip">{{ userInfo.IP }}</view>

			<view class="address">来自于：{{ userInfo.address?.city || userInfo.address?.province || userInfo.address?.country }}</view>
		</view>

		<view class="section">
			<view class="list">
				<view class="row" @click="navigateTo('/pages/classlist/classlist?name=我的下载&type=download')">
					<view class="left">
						<uni-icons type="download-filled" size="30rpx"></uni-icons>
						<view class="text">我的下载</view>
					</view>
					<view class="right">
						<view class="text">{{ userInfo.downloadSize }}</view>
						<uni-icons type="right" size="30rpx" color="#aaa"></uni-icons>
					</view>
				</view>
				<view class="row" @click="navigateTo('/pages/classlist/classlist?name=我的评分&type=score')">
					<view class="left">
						<uni-icons type="star-filled" size="30rpx"></uni-icons>
						<view class="text">我的评分</view>
					</view>
					<view class="right">
						<view class="text">{{ userInfo.scoreSize }}</view>
						<uni-icons type="right" size="30rpx" color="#aaa"></uni-icons>
					</view>
				</view>
				<view class="row">
					<view class="left">
						<uni-icons type="chatboxes-filled" size="30rpx"></uni-icons>
						<view class="text">联系客服</view>
					</view>
					<view class="right">
						<view class="text"></view>
						<uni-icons type="right" size="30rpx" color="#aaa"></uni-icons>
					</view>
					<!-- #ifdef MP-->
					<button open-type="contact">联系客服</button>
					<!-- #endif -->
					<!-- #ifndef MP-->
					<button @click="onCall">拨打电话</button>
					<!-- #endif -->
				</view>
			</view>
		</view>

		<view class="section">
			<view class="list">
				<view class="row" @click="navigateTo('/pages/notice/detail?id=65361e318b0da4ca084e3ce0')">
					<view class="left">
						<uni-icons type="notification-filled" size="30rpx"></uni-icons>
						<view class="text">关于我们</view>
					</view>
					<view class="right">
						<view class="text"></view>
						<uni-icons type="right" size="30rpx" color="#aaa"></uni-icons>
					</view>
				</view>
				<view class="row" @click="navigateTo('/pages/notice/detail?id=6536358ce0ec19c8d67fbe82')">
					<view class="left">
						<uni-icons type="flag-filled" size="30rpx"></uni-icons>
						<view class="text">常见问题</view>
					</view>
					<view class="right">
						<view class="text"></view>
						<uni-icons type="right" size="30rpx" color="#aaa"></uni-icons>
					</view>
				</view>
			</view>
		</view>
	</view>
</template>

<script setup>
import { ref } from 'vue';
import { getNavBarHeight } from '../../utils/system.js';
import { apiGetUserInfo } from '../../api/apis.js';

const userInfo = ref({});

function onCall() {
	uni.makePhoneCall({
		phoneNumber: '114'
	});
}

function navigateTo(url) {
	uni.navigateTo({
		url
	});
}

const getUserInfo = () => {
	apiGetUserInfo().then((res) => {
		userInfo.value = res.data;
	});
};

getUserInfo();
</script>

<style lang="scss" scoped>
.userLayout {
	.userInfo {
		display: flex;
		align-items: center;
		justify-content: center;
		flex-direction: column;
		padding: 50rpx 0;
		image {
			width: 160rpx;
			height: 160rpx;
			border-radius: 50%;
		}

		.ip {
			font-size: 44rpx;
			color: #333;
			padding: 20rpx 0 5rpx;
		}

		.address {
			font-size: 28rpx;
			color: #aaa;
		}
	}

	.section {
		width: 690rpx;
		margin: 50rpx auto;
		border: 1px solid #eee;
		border-radius: 10rpx;
		// border: 1px solid #000;
		// border-radius: 50rpx;

		box-shadow: 0 0 30rpx rgba(0, 0, 0, 0.05);
		.list {
			.row {
				display: flex;
				align-items: center;
				justify-content: space-between;
				height: 100rpx;
				padding: 0 30rpx;
				border-bottom: 1px solid #eee;
				// background: #fff;
				position: relative;
				&:last-child {
					border-bottom: 0;
				}

				.left,
				.right {
					display: flex;
					align-items: center;
					// justify-content: center;
				}

				.left {
					:deep() {
						.uni-icons {
							color: $brand-theme-color !important;
						}
					}
					.text {
						padding-left: 20rpx;
						color: #666;
					}
				}

				.right {
					.text {
						font-size: 28rpx;
						color: #aaa;
					}
				}

				button {
					width: 690rpx;
					height: 100%;
					position: absolute;
					top: 0;
					left: 0;
					opacity: 0;
				}
			}
		}
	}
}
</style>
