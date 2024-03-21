<template>
	<view>
		看看变量"a"变化了吗：a = {{a}}
	</view>
	<view>
		看看变量"b"变化了吗：b = {{b}}
	</view>
	<view>
		数组：{{ary}}
	</view>
	<view>
		对象：{{obj}}
	</view>
</template>

<script setup>
	import {
		ref
	} from 'vue'

	let a = 10;
	// setInterval(() => {
	// 	a++;
	// 	console.log('a：', a);
	// }, 1000)

	let b = ref(10);
	// setInterval(() => {
	// 	b.value++;
	// 	console.log('b：', b.value);
	// }, 1000)

	let ary = ref(['a', 'b', 'c', 'd']);
	setTimeout(() => {
		ary.value.push('e');
	}, 2000);

	let obj = ref({
		name: "zzx",
		age : 23
	});
	setTimeout(() => {
		obj.value.age = 100;
	}, 2000);
</script>

<style>

</style>