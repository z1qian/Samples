<template>
	<view class="out">
		<view class="box"></view>
		<view class="block"></view>
		<icon type="waiting" size="26" />
		<icon type="success" size="26" />
		<view>uniapp</view>
		<view>web前端开发</view>
		<text selectable user-select>nodejs span</text>
		<text space="emsp" decode>vue    &gt;        js</text>
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
</style>