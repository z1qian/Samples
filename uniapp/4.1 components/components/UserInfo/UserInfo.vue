<template>
	<view class="userinfo">
		<image :src="obj.avatar" mode=""></image>
		<view class="username">{{ obj.name }}</view>
	</view>
</template>

<script setup>
import { computed } from 'vue';
// const props = defineProps(['userName', 'avatar']);
// const props = defineProps({
// 	userName: [String, Number],
// 	avatar: {
// 		type: String,
// 		default: '/static/logo.png'
// 	}
// });
const props = defineProps({
	obj: {
		type: Object,
		default() {
			return { name: '匿名', avatar: '/static/logo.png' };
		}
	}
});

// const myName = computed(() => {
// 	// console.log('【myName】重新计算', props.userName);
// 	return `${props.userName}@`;
// });
</script>

<style lang="scss" scoped>
.userinfo {
	width: 100%;
	height: 200px;
	background-color: #ccc;
	display: flex;
	align-items: center;
	justify-content: center;
	flex-direction: column;

	image {
		width: 100px;
		height: 100px;
		border-radius: 50%;
	}

	.username {
		padding: 10px 0;
		font-size: 20px;
	}
}
</style>
