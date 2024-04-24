<template>
	<view class="">
		<zzx-defineExpose ref="child"></zzx-defineExpose>
		<button @click="onClick" type="primary">改变子组件的值</button>
		<navigator url="/pages/onLoad/onLoad?name=王五&age=20">跳转到onLoad页面</navigator>
	</view>
</template>

<script setup>
import { onMounted, ref } from 'vue';
const child = ref(null);
onMounted(() => console.log('onMounted', child.value));

function onClick() {
	child.value.changeCount();
}
</script>

<style lang="scss" scoped></style>
