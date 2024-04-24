<template>
	<view class="">
		<view v-for="item in 100">{{ item }}</view>
	</view>
</template>

<script setup>
import { ref } from 'vue';
import { onReachBottom } from '@dcloudio/uni-app';

onReachBottom(() => console.log('到底啦'));
</script>

<style lang="scss" scoped></style>
