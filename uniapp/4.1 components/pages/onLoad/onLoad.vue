<template>
	<view class="">
		姓名：{{ name }} - {{ age }}
		<scroll-view scroll-y="true" ref="scroll"></scroll-view>
		<navigator url="/pages/defineExpose/defineExpose">导航到defineExpose页面</navigator>

		{{ num }}
		<navigator url="/pages/emit/emit" open-type="reLaunch">导航到emit页面</navigator>

		<view class="" v-for="item in 50">
			{{ item }}
		</view>
		<view class="flex" v-if="show">👆</view>
	</view>
</template>

<script setup>
// import { onBeforeMount, onMounted, ref } from 'vue';
// import { onReady, onLoad, onShow, onHide, onPageScroll, onUnload } from '@dcloudio/uni-app';

const scroll = ref(null);

const name = ref('');
const age = ref('');

onLoad((e) => {
	console.log('onLoad', scroll.value);
	console.log('onLoad', e);

	name.value = e.name;
	age.value = e.age;
});

onReady(() => {
	console.log('onReady', scroll.value);
});

onShow(() => {
	console.log('onShow', new Date().toLocaleString('zh-CN', { timeZone: 'Asia/Shanghai' }));

	// time = setInterval(() => {
	// 	num.value++;
	// }, 100);
	console.log('onShow', scroll.value);
});

onHide(() => {
	console.log('onHide', new Date().toLocaleString('zh-CN', { timeZone: 'Asia/Shanghai' }));

	// clearInterval(time);
});

const num = ref(0);
// let time = setInterval(() => {
// 	num.value++;
// }, 100);

onMounted(() => console.log('onMounted'));

onBeforeMount(() => console.log('onBeforeMount', scroll.value));

onUnload(() => {
	console.log('onUnload');
});

onPageScroll((e) => {
	console.log('onPageScroll', e.scrollTop);
	show.value = e.scrollTop > 200;
});

const show = ref(false);
</script>

<style lang="scss" scoped>
.flex {
	width: 100px;
	height: 100px;
	background-color: red;
	position: fixed;
	right: 30px;
	bottom: 30px;
}
</style>