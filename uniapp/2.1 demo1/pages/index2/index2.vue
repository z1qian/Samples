<template>
	<view class="out">
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
		
		<swiper class="swiper" >
			<swiper-item  class="item">
				<image src="../../static/images/pic1.jpg"></image>
			</swiper-item>
			<swiper-item  class="item">
				<image src="../../static/images/pic2.jpg"></image>
			</swiper-item>
			<swiper-item  class="item">
				<image src="../../static/images/pic3.png"></image>
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
</style>