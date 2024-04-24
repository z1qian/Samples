<template>
	<view class="noticeLayout">
		<view class="title">
			<view class="tag" v-if="noticeDetail.select">
				<uni-tag inverted text="置顶" type="error" />
			</view>
			<view class="font">{{ noticeDetail.title }}</view>
		</view>

		<view class="info">
			<view class="item">{{ noticeDetail.author }}</view>
			<view class="item">
				<uni-dateformat :date="noticeDetail.publish_date" format="yyyy-MM-dd hh:mm:ss"></uni-dateformat>
			</view>
		</view>

		<!-- <view class="content" v-html="noticeDetail.content"></view> -->
		<view class="content">
			<mp-html :content="noticeDetail.content" />
		</view>

		<view class="count">阅读 {{ noticeDetail.view_count }}</view>
	</view>
</template>

<script setup>
import { ref } from 'vue';
import { apiGetwallNewsDetail } from '../../api/apis.js';
import { onLoad } from '@dcloudio/uni-app';

const noticeDetail = ref({});
let noticeId = 0;

onLoad((e) => {
	noticeId = e.id;

	getwallNewsDetail();
});

const getwallNewsDetail = () => {
	apiGetwallNewsDetail({
		id: noticeId
	}).then((res) => {
		noticeDetail.value = res.data;
	});
};
</script>

<style lang="scss" scoped>
.noticeLayout {
	padding: 30rpx;
	.title {
		font-size: 40rpx;
		color: #111;
		line-height: 1.6em;
		padding-bottom: 30rpx;
		display: flex;
		.tag {
			transform: scale(0.8);
			transform-origin: left center;
			flex-shrink: 0;
		}
		.font {
			padding-left: 6rpx;
		}
	}
	.info {
		display: flex;
		align-items: center;
		color: #999;
		font-size: 28rpx;
		.item {
			padding-right: 20rpx;
		}
	}
	.content {
		padding: 50rpx 0;
	}
	.count {
		color: #999;
		font-size: 28rpx;
	}
}
</style>
