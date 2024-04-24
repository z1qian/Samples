<template>
	<view class="out">
		<!-- 	<input type="text" @focus="isActive=true" @blur="isActive=false" @input="e=>iptValue=e.detail.value" /> -->
		<input type="text" @focus="isActive=true" @blur="isActive=false" v-model="iptValue" @confirm="onConfirm" />
		<image src="../../static/images/chicken.gif" mode="" class="pic" :class="{active:isActive}"></image>

		<view style="margin-top: 10px;">预览：{{iptValue}}</view>
	</view>
</template>

<script setup>
	import {
		ref
	} from 'vue';

	let isActive = ref(false);

	// 预览
	let iptValue = ref("");

	setTimeout(() => {
		iptValue.value = "您长时间未进行输入"
	}, 2000);

	// @confirm
	function onConfirm(e) {
		console.log('【confirm】', e.detail.value);
	}
</script>

<style lang="scss" scoped>
	.out {
		padding: 0px 20px;
		margin-top: 40px;
		position: relative;

		input {
			border: 1px solid #ccc;
			height: 40px;
			position: relative;
			z-index: 2;
			background: #fff;
			padding: 0 10px;
		}

		.pic {
			width: 24px;
			height: 24px;
			z-index: 1;
			position: absolute;
			top: 0px;
			left: 50%;
			transform: translateX(-50%);
			transition: top 0.3s;
		}

		.pic.active {
			top: -24px;
		}
	}
</style>