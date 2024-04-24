<template>
	<view>
		<view v-if="shop">淘宝</view>
		<view v-else>京东</view>

		<button type="primary" @click="onClick">在哪个平台购物？</button>
	</view>

	<view>
		<view v-if="day===1">星期一</view>
		<view v-else-if="day===2">星期二</view>
		<view v-else-if="day===3">星期三</view>
		<view v-else-if="day===4">星期四</view>
		<view v-else-if="day===5">星期五</view>
		<view v-else-if="day===6">星期六</view>
		<view v-else-if="day===0">星期日</view>
		<view v-else>星期八</view>
	</view>

	<view class="box" v-if="false">
		1
	</view>

	<view class="box" v-show="false">
		2
	</view>

	<!-- 默认分割线 -->

	<template v-if="false">
		<image src="../../static/images/pic1.jpg" mode=""></image>
		<view>一个老人</view>
	</template>


	<template v-else>
		<image src="../../static/images/pic2.jpg" mode=""></image>
		<view>一个XX</view>
	</template>

</template>

<script setup>
	import {
		ref
	} from 'vue';

	// 在哪个平台购物
	const shop = ref(false);

	function onClick() {
		shop.value = !shop.value;
	}

	//今天星期几
	const day = new Date().getDay();
</script>

<style lang="scss" scoped>
	.box {
		width: 100px;
		height: 100px;
		background-color: green;
		margin: 5px 0px;
	}
</style>