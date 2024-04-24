<template>
	<view class="">
		<input type="text" placeholder="请输入名" v-model="firstName" />
		<input type="text" placeholder="请输入姓" v-model="lastName" />

		<view class="">
			<!-- 全称：{{firstName}}-{{lastName}} -->
			<!-- 全称：{{fullName}} -->
			全称：{{fullName}}
		</view>
		<view class="">
			全称：{{fullName}}
		</view>
		<view class="">
			全称：{{fullName}}
		</view>
	</view>
</template>

<script setup>
	import {
		ref,
		computed
	} from 'vue';

	let firstName = ref("杰姆斯");
	let lastName = ref("邦德");

	let fullName = computed(() => {
		console.log("计算属性");
		return `${firstName.value}-${lastName.value}`;
	});

	// const fullName = () => {
	// 	console.log("方法");
	// 	return `${firstName.value}-${lastName.value}`;
	// };

	// 计算属性应该是只读的
	// fullName.value = "123";

	// 应该更新它所依赖的源状态以触发新的计算
	firstName.value = "123";
	console.log('修改后的值：', fullName.value);
</script>

<style lang="scss" scoped>
	input {
		height: 40px;
		border: 1px solid #ccc;
		padding: 0px 10px;
		margin-bottom: 10px;
	}
</style>