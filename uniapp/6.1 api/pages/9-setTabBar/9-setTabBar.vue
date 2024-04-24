<template>
	<view class="">
		<button type="primary" @click="setTabBarItem">动态设置 tabBar 某一项的内容</button>
		<button type="primary" @click="setTabBarStyle">动态设置 tabBar 的整体样式</button>
		<button type="primary" @click="hideTabBar">隐藏 tabBar</button>
		<button type="primary" @click="showTabBar">显示 tabBar</button>
		<button type="primary" @click="setTabBarBadge">为 tabBar 某一项的右上角添加文本</button>
		<button type="primary" @click="removeTabBarBadge">移除 tabBar 某一项右上角的文本</button>
		<button type="primary" @click="showTabBarRedDot">显示 tabBar 某一项的右上角的红点</button>
		<button type="primary" @click="hideTabBarRedDot">隐藏 tabBar 某一项的右上角的红点</button>
	</view>
</template>

<script setup>
import { ref } from 'vue';

function setTabBarItem() {
	uni.setTabBarItem({
		index: 1,
		text: '动态改变tab上的文字',
		success(e) {
			console.log('success', e);
		}
	});
}

function setTabBarStyle() {
	uni.setTabBarStyle({
		color: '#FFC0CB',
		backgroundColor: '#0000FF',
		success(e) {
			console.log(e);
		}
	});
}

function hideTabBar() {
	uni.hideTabBar({
		animation: true,
		success(e) {
			console.log('success', e);
		}
	});
}

function showTabBar() {
	uni.showTabBar({
		animation: true,
		success(e) {
			console.log('success', e);
		}
	});
}

function setTabBarBadge() {
	uni.setTabBarBadge({
		index: 1,
		text: 'asd'
	});
}

function removeTabBarBadge() {
	uni.removeTabBarBadge({ index: 1 });
}

function showTabBarRedDot() {
	uni.showTabBarRedDot({
		index: 0
	});
}

function hideTabBarRedDot() {
	uni.hideTabBarRedDot({
		index: 0
	});
}
</script>

<style lang="scss" scoped></style>
