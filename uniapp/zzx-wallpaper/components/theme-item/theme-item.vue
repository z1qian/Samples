<template>
	<view class="themeItem">
		<navigator :url="`/pages/classlist/classlist?id=${item._id}&name=${item.name}`" class="box" v-if="!isMore">
			<image :src="item.picurl" class="pic"></image>
			<view class="mask">{{ item.name }}</view>
			<view class="tab" v-if="compareTime(item.updateTime)">{{ compareTime(item.updateTime) }}更新</view>
		</navigator>

		<navigator url="/pages/classify/classify" open-type="reLaunch" class="box more" v-else>
			<image src="../../common/images/more.jpg" class="pic"></image>
			<view class="mask">
				<uni-icons type="more-filled" size="34rpx" color="#fff"></uni-icons>
				<view class="text">更多</view>
			</view>
		</navigator>
	</view>
</template>

<script setup>
import { ref } from 'vue';
import { compareTime } from '../../utils/common.js';

defineProps({
	isMore: {
		type: Boolean,
		default: false
	},
	item: {
		type: Object,
		// 对象或数组的默认值
		// 必须从一个工厂函数返回。
		// 该函数接收组件所接收到的原始 prop 作为参数。
		default(rawProps) {
			return {
				name: '默认名称',
				picurl: '../../common/images/classify1.jpg',
				updateTime: Date.now()
			};
		}
	}
});
</script>

<style lang="scss" scoped>
.themeItem {
	.box {
		width: 220rpx;
		height: 340rpx;
		border-radius: 10rpx;
		overflow: hidden;
		position: relative;
		.pic {
			width: 100%;
			height: 100%;
		}

		.mask {
			width: 100%;
			height: 70rpx;
			position: absolute;
			bottom: 0;
			left: 0;
			display: flex;
			align-items: center;
			justify-content: center;
			background: rgba(0, 0, 0, 0.2);
			color: #fff;
			backdrop-filter: blur(20rpx);
			font-weight: 600;
			font-size: 30rpx;
		}

		.tab {
			position: absolute;
			top: 0;
			left: 0;
			background: rgba(250, 129, 90, 0.7);
			backdrop-filter: blur(20rpx);
			color: #fff;
			font-size: 22rpx;
			padding: 6rpx 14rpx;
			border-radius: 0 0 20rpx 0;
			transform: scale(0.8);
			transform-origin: left top;
		}
	}
	.box.more {
		.mask {
			height: 100%;
			flex-direction: column;

			.text {
				font-size: 28rpx;
			}
		}
	}
}
</style>
