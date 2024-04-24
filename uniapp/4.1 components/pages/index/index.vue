<template>
	<view class="content">
		<!-- <UserInfo v-for="item in 2"></UserInfo> -->
		<!-- <zzx-header v-for="item in 3"></zzx-header> -->
		<!-- <UserInfo :obj="{ name: '张三', avatar: '/static/pic1.jpg' }"></UserInfo>
		<UserInfo :obj="lisiObj"></UserInfo>
		<UserInfo :obj="{ name: '王五', avatar: '/static/pic2.jpg' }"></UserInfo> -->

		<UserInfo></UserInfo>
		<UserInfo v-for="item in userinfo" :obj="item"></UserInfo>
	</view>
</template>

<script setup>
import { ref } from 'vue';
// const name = ref('王五');

// setTimeout(() => (name.value = '赵六'), 2000);
// const lisiObj = {
// 	name: '李四',
// 	avatar: '/static/pic3.jpg'
// };

const userinfo = ref([
	{ name: '张三', avatar: '../../static/pic1.jpg' },
	{ name: '李四', avatar: '../../static/pic2.jpg' },
	{ name: '王五', avatar: '../../static/pic3.jpg' }
]);
</script>

<style></style>
