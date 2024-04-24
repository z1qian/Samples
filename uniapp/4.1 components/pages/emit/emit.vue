<template>
	<view class="">
		<zzx-child @add="onAdd" @change="onChange"></zzx-child>
		<view class="box" :style="{ backgroundColor: colorValue, fontSize: size }">
			{{ num }}
		</view>

		字体大小：{{ size }}
	</view>
</template>

<script setup>
import { ref } from 'vue';

const num = ref(0);
const colorValue = ref('#ccc');

function onAdd(e) {
	num.value = e;
	colorValue.value = `#${e.toString().substring(2, 8)}`;
}

const size = ref('14px');

function onChange(e) {
	size.value = `${e}px`;
}
</script>

<style lang="scss" scoped>
.box {
	width: 200px;
	height: 200px;
}
</style>
