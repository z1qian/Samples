<template>
	<view class="">
		<swiper circular :current="currentIndex">
			<swiper-item v-for="(item, index) in 10" :key="index">
				<view>{{ item }}</view>
			</swiper-item>
		</swiper>

		{{ Date.now() }}
	</view>
</template>

<script setup>
console.log('setup', Date.now());
import { ref } from 'vue';
import { onLoad } from '@dcloudio/uni-app';

const currentIndex = ref(0);

onLoad(() => {
	// currentIndex.value = 1;
	console.log('onLoad', Date.now());

	setTimeout(() => {
		currentIndex.value = 1;
	}, 5000);
});
</script>

<style lang="scss" scoped></style>
