<template>
	<view class="out">
		<image src="../../static/images/circle.jpg" mode="scaleToFill" class="myimg"></image>
		<image src="../../static/images/shuijiao.jpg" mode="heightFix" class=""></image>

		<view class="box"></view>
		<view class="block"></view>
		<icon type="waiting" size="26" />
		<icon type="success" size="26" />
		<view>uniapp</view>
		<view>web前端开发</view>
		<text selectable user-select>nodejs span</text>
		<text space="emsp" decode>vue &gt; js</text>

		<scroll-view class="scroll" scroll-x scroll-y>
			<view class="group">
				<view class="item">
					111
				</view>

				<view class="item">
					222
				</view>

				<view class="item">
					333
				</view>

				<view class="item">
					444
				</view>

				<view class="item">
					555
				</view>
			</view>
		</scroll-view>

		<swiper class="swiper" indicator-dots autoplay interval=3000 duration=300 circular>
			<swiper-item class="item">
				<image src="../../static/images/pic1.webp" mode="scaleToFill"></image>
			</swiper-item>
			<swiper-item class="item">
				<image src="../../static/images/pic2.webp" mode="scaleToFill"></image>
			</swiper-item>
			<swiper-item class="item">
				<image src="../../static/images/pic3.webp" mode="scaleToFill"></image>
			</swiper-item>
		</swiper>
	</view>
</template>

<script>
	export default {
		data() {
			return {

			};
		}
	}
</script>

<style lang="scss">
	.out {
		.box {
			width: 200px;
			height: 200px;
			background: pink;
		}

		.block {
			width: 750rpx;
			height: 200rpx;
			background: green;
		}
	}

	.scroll {
		border: 1px solid red;
		box-sizing: border-box;
		height: 100rpx;

		.group {
			white-space: nowrap;

			.item {
				width: 220rpx;
				height: 220rpx;
				background: royalblue;
				display: inline-block;
				margin-right: 10rpx;
			}
		}
	}

	.swiper {
		height: 400rpx;
		margin-top: 100rpx;
		
		.item{
			padding: 20rpx;
			box-sizing: border-box;
			image{
				width: 100%;
				height: 350rpx;
			}
		}
	}

	.myimg {
		width: 200rpx;
		height: 200rpx;
		background-color: red;
	}
</style>