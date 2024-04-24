<template>
	<view class="">
		<input type="text" v-model="person.name" />
	</view>

	{{ person }}

	<view>--------------------------------</view>

	<view>
		<input type="text" placeholder="请输入名" v-model="firstName" />
		<input type="text" placeholder="请输入姓" v-model="lastName" />
	</view>
</template>

<script setup>
import { ref, watch, watchEffect } from 'vue';

const person = ref({
	name: 'zzx',
	age: 23
});

// watch(() => person.value.name, (val) => {
// 	console.log("val：", val);
// });
watch(
	person,
	(val) => {
		console.log('val：', val);
	},
	{
		deep: true,
		immediate: true
	}
);

let firstName = ref('杰姆斯');
let lastName = ref('邦德');

// watch([firstName, lastName], (
// 	[nfirstName, nlastName],
// 	[ofirstName, olastName]) => {
// 	console.log('新：', nfirstName, nlastName);
// 	console.log('旧：', ofirstName, olastName);
// });

// watch(firstName, (nv, ov) => {
// 	console.log(nv, ov);
// });

// watch(lastName, (nv, ov) => {
// 	console.log(nv, ov);
// });

watchEffect(() => {
	console.log('watchEffect');
	console.log(firstName.value, lastName.value);
});
</script>

<style lang="scss" scoped>
input {
	height: 40px;
	border: 1px solid #ccc;
	padding: 0px 10px;
	margin: 0px px 10px 10px;
}
</style>
