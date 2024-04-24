<template>
	<view class="out">子组件count值：{{ count }}</view>
</template>

<script setup>
import { ref } from 'vue';
const count = ref(100);

const changeCount = () => count.value++;

defineExpose({
	count,
	str: 'zzx',
	changeCount
});
</script>

<style lang="scss" scoped>
.out {
	padding: 10px;
	background-color: #ccc;
	margin-bottom: 10px;
	border-bottom: 1px solid red;
}
</style>
