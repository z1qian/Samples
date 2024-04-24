<template>
	<view>
		子组件
		<!-- <button @click="$emit('add', Math.random())">按钮</button> -->
		<button @click="onClick">按钮</button>
		<view>------我是分割线------</view>
		<input type="number" @input="onInput" placeholder="请输入数字" />
	</view>
</template>

<script setup>
const emit = defineEmits(['add', 'change']);
function onClick() {
	emit('add', Math.random());
}

function onInput(e) {
	emit('change', e.detail.value);
}
</script>

<style lang="scss" scoped>
input {
	border: 1px solid #ccc;
	height: 40px;
	margin-bottom: 10px;
	padding: 0px 5px;
}
</style>
