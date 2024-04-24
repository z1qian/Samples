<template>
	<view class="previewLayout" v-if="currentIndex != -1">
		<swiper circular :current="currentIndex" @change="picChange">
			<swiper-item v-for="(item, index) in classList" :key="item._id">
				<image :src="item.picurl" mode="aspectFill" @click="maskState = !maskState" v-if="readIndexList.includes(index)"></image>
			</swiper-item>
		</swiper>

		<view class="mask" v-if="maskState">
			<view class="goBack" :style="{ top: `${getStatusBarHeight()}px`, height: `${getTitleBarHeight()}px`, width: `${getTitleBarHeight()}px` }" @click="goBack">
				<uni-icons type="back" color="#fff" size="38rpx"></uni-icons>
			</view>
			<view class="count">{{ currentIndex + 1 }} / {{ classList.length }}</view>
			<view class="time">
				<uni-dateformat format="hh:mm" :date="Date.now()"></uni-dateformat>
			</view>
			<view class="date">
				<uni-dateformat format="MM月dd日" :date="Date.now()"></uni-dateformat>
			</view>
			<view class="footer">
				<view class="box" @click="popupInfo">
					<uni-icons type="info" size="35rpx"></uni-icons>
					<view class="text">信息</view>
				</view>
				<view class="box" @click="popupScore">
					<uni-icons type="star" size="35rpx"></uni-icons>
					<view class="text">{{ currentInfo.score }}分</view>
				</view>
				<view class="box" @click="download">
					<uni-icons type="download" size="32rpx"></uni-icons>
					<view class="text">下载</view>
				</view>
			</view>
		</view>

		<uni-popup ref="infoPopup" type="bottom" background-color="#fff" borderRadius="30rpx 30rpx 0 0">
			<view class="infoPopup">
				<view class="popHeader">
					<view></view>
					<view class="title">壁纸信息</view>
					<view class="close" @click="closePopupInfo">
						<uni-icons type="closeempty" size="36rpx" color="#999"></uni-icons>
					</view>
				</view>

				<scroll-view scroll-y>
					<view class="content">
						<view class="row">
							<view class="label">壁纸ID：</view>
							<text selectable class="value" user-select>{{ currentInfo._id }}</text>
						</view>

						<!-- 	<view class="row">
							<view class="label">分类：</view>
							<view class="value classify">明星美女</view>
						</view> -->

						<view class="row">
							<view class="label">发布者：</view>
							<view class="value">{{ currentInfo.nickname }}</view>
						</view>

						<view class="row">
							<view class="label">评分：</view>
							<view class="value roteBox">
								<uni-rate :value="currentInfo.score" size="40rpx" readonly />
								<view class="score">{{ currentInfo.score }}分</view>
							</view>
						</view>

						<view class="row">
							<view class="label">摘要：</view>
							<view class="value">{{ currentInfo.description }}</view>
						</view>

						<view class="row">
							<view class="label">标签：</view>
							<view class="value tabs">
								<view class="tab" v-for="tab in currentInfo.tabs" :key="tab">
									{{ tab }}
								</view>
							</view>
						</view>

						<view class="copyright">
							声明：本图片来用户投稿，非商业使用，用于免费学习交流，如侵犯了您的权益，您可以拷贝壁纸ID举报至平台，邮箱513894357@qq.com，管理将删除侵权壁纸，维护您的权益。
						</view>
					</view>
				</scroll-view>
			</view>
		</uni-popup>

		<uni-popup ref="scorePopup" type="center" background-color="#fff" borderRadius="30rpx" :is-mask-click="false">
			<view class="scorePopup">
				<view class="popHeader">
					<view></view>
					<view class="title">{{ isScored ? '已评分过了~' : '壁纸评分' }}</view>
					<view class="close" @click="closePopupScore">
						<uni-icons type="closeempty" size="36rpx" color="#999"></uni-icons>
					</view>
				</view>

				<view class="content">
					<uni-rate v-model="myScore" allowHalf :readonly="isScored" size="30" />
					<text class="text">{{ myScore }}分</text>
				</view>

				<view class="fotter">
					<button type="default" size="mini" plain :disabled="!myScore || isScored" @click="score">确定评分</button>
				</view>
			</view>
		</uni-popup>
	</view>
</template>

<script setup>
import { ref, computed, watch } from 'vue';
import { getStatusBarHeight, getTitleBarHeight } from '../../utils/system.js';
import { onLoad, onShareAppMessage, onShareTimeline } from '@dcloudio/uni-app';
import { apiSetupScore, apiDownloadWall, apiGetDetailWall } from '/api/apis.js';

const maskState = ref(true);
const infoPopup = ref(null);
const scorePopup = ref(null);
const myScore = ref(0);
const classList = ref([]);
const currentIndex = ref(-1);
const readIndexList = ref([]);
const isScored = ref(false);

const currentInfo = computed(() => {
	console.log('currentInfo被重新计算', 'index：' + currentIndex.value);
	return classList.value[currentIndex.value];
});
watch(currentInfo, (newValue) => {
	changeCurrentInfoEvent(newValue);
});

let id = null;

onLoad(async (e) => {
	id = e.id || 0;

	if (e.type == 'share') {
		const res = await apiGetDetailWall({ id });
		classList.value = res.data.map((item) => {
			return {
				...item,
				picurl: item.smallPicurl.replace('_small.webp', '.jpg')
			};
		});
	}

	currentIndex.value = classList.value.findIndex((item) => item._id == id);
	loadNearbyPicIndex(currentIndex.value);
});

// 壁纸左右切换事件
const picChange = (event) => {
	const {
		detail: { current = 0 }
	} = event;

	currentIndex.value = current;
	loadNearbyPicIndex(current);
};

// 弹出信息popup
function popupInfo() {
	infoPopup.value.open();
}

// 关闭信息popup
function closePopupInfo() {
	infoPopup.value.close();
}

// 弹出评分popup
function popupScore() {
	scorePopup.value.open();
}

// 关闭评分popup
function closePopupScore() {
	scorePopup.value.close();
}

// 壁纸评分
async function score() {
	uni.showLoading({
		title: '加载中...'
	});
	const userScore = myScore.value;
	const { classid, _id: wallId } = currentInfo.value;

	const res = await apiSetupScore({
		classid,
		wallId,
		userScore
	});
	uni.hideLoading();

	// 评分成功，没有多次对此壁纸评分
	currentInfo.value.userScore = userScore;
	changeCurrentInfoEvent(currentInfo.value);
	uni.setStorageSync('storgClassList', classList.value);

	uni.showToast({
		title: res.errMsg,
		icon: 'none'
	});
	closePopupScore();
}

// 返回按钮
function goBack() {
	uni.navigateBack({
		fail() {
			uni.reLaunch({
				url: '/pages/index/index'
			});
		}
	});
}

// 从缓存中读出壁纸信息列表
const storgClassList = uni.getStorageSync('storgClassList') || [];
classList.value = storgClassList.map((item) => {
	return {
		...item,
		picurl: item.smallPicurl.replace('_small.webp', '.jpg')
	};
});

// 加载相邻的图片
function loadNearbyPicIndex(index) {
	readIndexList.value.push(index <= 0 ? classList.value.length - 1 : index - 1, index, index >= classList.value.length - 1 ? 0 : index + 1);

	readIndexList.value = [...new Set(readIndexList.value)];
}

function changeCurrentInfoEvent(newValue) {
	isScored.value = 'userScore' in newValue;
	myScore.value = isScored.value ? newValue.userScore : 0;
}

// 下载壁纸
async function download() {
	// #ifdef H5
	uni.showModal({
		content: '请长按保存壁纸',
		showCancel: false
	});
	// #endif

	// #ifndef H5
	uni.showLoading({
		title: '下载中...',
		mask: true
	});

	const { classid, _id: wallId } = currentInfo.value;
	let apiRes = null;

	try {
		apiRes = await apiDownloadWall({
			classid,
			wallId
		});
	} catch (e) {
		uni.hideLoading();
		return;
	}

	uni.getImageInfo({
		src: currentInfo.value.picurl,
		success: (res) => {
			uni.saveImageToPhotosAlbum({
				filePath: res.path,
				fail(res) {
					// 用户首次点击拒绝授权
					if (res.errMsg == 'saveImageToPhotosAlbum:fail auth deny') {
						uni.showModal({
							title: '授权提示',
							content: '需要授权保存相册',
							showCancel: false,
							success(res) {
								uni.openSetting({
									success(setting) {
										if (setting.authSetting['scope.writePhotosAlbum']) {
											uni.showToast({
												title: '获取授权成功'
											});
										} else {
											uni.showToast({
												title: '获取授权失败',
												icon: 'error'
											});
										}
									}
								});
							}
						});
					}
					// 授权完毕保存时，点击了取消按钮
					else if (res.errMsg == 'saveImageToPhotosAlbum:fail cancel') {
						uni.showToast({
							title: '请重新点击下载',
							icon: 'error'
						});
					} else {
						uni.showToast({
							title: '未知错误',
							icon: 'error'
						});
					}
				},
				success(res) {
					uni.showToast({
						title: '保存成功'
					});
				}
			});
		},
		complete() {
			uni.hideLoading();
		}
	});

	// #endif
}

// 发送给朋友
onShareAppMessage((e) => {
	return {
		title: 'zzx壁纸-壁纸预览',
		path: `/pages/preview/preview?id=${id}&type=share`
	};
});

// 分享到朋友圈
onShareTimeline(() => {
	return {
		title: '分享到你的朋友圈-壁纸预览',
		query: `id=${id}&type=share`,
		imageUrl: currentInfo.value.picurl
	};
});
</script>

<style lang="scss" scoped>
.previewLayout {
	width: 100%;
	height: 100vh;
	position: relative;
	swiper {
		width: 100%;
		height: 100%;

		image {
			width: 100%;
			height: 100%;
		}
	}

	.mask {
		// mask下的子元素（view），不含孙元素
		& > view {
			position: absolute;
			left: 0;
			right: 0;
			margin: auto;
			width: fit-content;
			color: #fff;
		}

		.goBack {
			width: 76rpx;
			height: 76rpx;
			background: rgba(0, 0, 0, 0.5);
			left: 30rpx;
			margin-left: 0;
			border-radius: 76rpx;
			top: 0;
			backdrop-filter: blur(10rpx);
			border: 1rpx solid rgba(255, 255, 255, 0.3);
			display: flex;
			align-items: center;
			justify-content: center;
		}

		.count {
			top: 10vh;
			background: rgba(0, 0, 0, 0.3);
			font-size: 28rpx;
			border-radius: 40rpx;
			padding: 8rpx 28rpx;
			backdrop-filter: blur(10rpx);
		}
		.time {
			top: calc(10vh + 80rpx);
			font-size: 140rpx;
			font-weight: 100;
			line-height: 1em;
			text-shadow: 0 4rpx rgba(0, 0, 0, 0.3);
		}
		.date {
			font-size: 34rpx;
			top: calc(10vh + 230rpx);
			text-shadow: 0 2rpx rgba(0, 0, 0, 0.3);
		}

		.footer {
			background: rgba(255, 255, 255, 0.8);
			bottom: 10vh;
			width: 65vw;
			height: 120rpx;
			border-radius: 60rpx;
			color: #000;
			display: flex;
			justify-content: space-around;
			align-items: center;
			box-shadow: 0 2rpx 0 rgba(0, 0, 0, 0.1);
			backdrop-filter: blur(20rpx);

			.box {
				display: flex;
				flex-direction: column;
				align-items: center;
				justify-content: center;
				padding: 2rpx 12rpx;
				.text {
					font-size: 26rpx;
					color: $text-font-color-2;
				}
			}
		}
	}

	// 公用的样式，提取出来
	.popHeader {
		display: flex;
		justify-content: space-between;
		align-items: center;
		.title {
			color: $text-font-color-2;
			font-size: 26rpx;
		}

		.close {
			padding: 6rpx;
		}
	}

	.infoPopup {
		padding: 30rpx;
		scroll-view {
			max-height: 60vh;
			.content {
				.row {
					display: flex;
					padding: 16rpx 0;
					font-size: 32rpx;
					line-height: 1.7em;
					.label {
						color: $text-font-color-3;
						width: 140rpx;
						text-align: right;
						font-size: 30rpx;
					}
					.value {
						flex: 1;
						width: 0;
					}

					.classify {
						color: $brand-theme-color;
					}

					.roteBox {
						display: flex;
						align-items: center;
						.score {
							font-size: 26rpx;
							color: $text-font-color-2;
							padding-left: 10rpx;
						}
					}

					.tabs {
						display: flex;
						flex-wrap: wrap;
						.tab {
							border: 1px solid $brand-theme-color;
							color: $brand-theme-color;
							font-size: 22rpx;
							padding: 10rpx 30rpx;
							border-radius: 40rpx;
							line-height: 1em;
							margin: 0 10rpx 10rpx 0;
						}
					}
				}

				.copyright {
					font-size: 28rpx;
					padding: 20rpx;
					background: #f6f6f6;
					color: #666;
					border-radius: 10rpx;
					margin: 20rpx 0;
					line-height: 1.6em;
				}
			}
		}
	}

	.scorePopup {
		padding: 30rpx;
		width: 70vw;
		.content {
			padding: 30rpx 0;
			display: flex;
			align-items: center;
			justify-content: center;
			.text {
				width: 80rpx;
				color: #ffca3e;
				padding-left: 10rpx;
				line-height: 1em;
				text-align: right;
			}
		}

		.fotter {
			padding: 10rpx 0;
			display: flex;
			align-items: center;
			justify-content: center;
		}
	}
}
</style>
