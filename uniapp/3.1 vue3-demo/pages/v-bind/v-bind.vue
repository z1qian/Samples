<template>
	<view>
		<image :src="picAdr"></image>
	</view>

	<view>
		<button size="mini" type="primary" :disabled="false">我是一个按钮</button>
	</view>

	<view class="box" :class="{active:isActive}">
		v-bind指令
	</view>

	<!-- <view class="box" :class="isActive?'active':''"></view> -->

	<view class="box" :style="{'font-size':size+'px',color:'yellow',width:'200px'}">
		内联样式
	</view>
</template>

<script setup>
	import {
		ref
	} from 'vue'

	// 图片切换小案例
	let picAry = ['/static/images/pic1.jpg',
		'/static/images/pic2.jpg',
		'/static/images/pic3.jpg',
		'/static/images/pic4.jpg'
	];

	let picAdr = ref(picAry[0]);
	let i = 0;
	setInterval(() => {
		i++;
		picAdr.value = picAry[i % 4];
	}, 2000);

	// :class 
	let isActive = ref(false);
	setTimeout(() => {
		isActive.value = true;
	}, 5000);

	//内联样式
	let size = ref(10);
	// setInterval(() => {
	// 	size.value++;
	// }, 2000);
</script>

<style lang="scss">
	.box {
		width: 100px;
		height: 100px;
		background-color: black;
		color: white;
	}

	.active {
		background-color: green;
		color: red;
	}
</style>