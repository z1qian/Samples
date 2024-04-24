<template>
	<view class="box">这是box</view>
	<view class="text">欢迎使用</view>
</template>

<script setup>
import { ref } from 'vue';
</script>

<style lang="scss" scoped>
.box {
	width: 200rpx;
	height: 200rpx;
	background-color: red;
	color: blue;
}

.text {
	font-size: 50rpx;
	color: $custom-color-1;
}

@import '@/common/scss/self.scss';
</style>
