<template>
	<view class="box" @click="onClick" :style="{backgroundColor:'#'+bgColor}">
		{{num}}
	</view>

	<view>
		<switch @change="onChange"></switch>
		<button type="primary" :loading="isLoading">按钮</button>
	</view>
</template>

<script setup>
	import {
		ref
	} from 'vue'

	const num = ref(10);
	const bgColor = ref(Math.random().toString().substring(2, 8));
	const isLoading = ref(false);

	function onClick() {
		num.value++;
		let randomStr = Math.random().toString();
		bgColor.value = randomStr.substring(2, 8);
	}

	function onChange(e) {
		isLoading.value=e.detail.value;
	}
</script>

<style>
	.box {
		width: 200px;
		height: 200px;
		background-color: aqua;
	}
</style>