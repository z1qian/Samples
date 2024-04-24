<template>
	<view class="list">
		<checkbox-group @change="onChange">
			<view class="item" v-for="(item,index) in goods" :key="item.id">
				<checkbox :value="item.id" :checked="item.checked"></checkbox>
				<text>{{item.name}}</text>
				<text class="price">{{item.price}}元</text>
				<text class="remove" @click="remove(index)">删除</text>
			</view>
		</checkbox-group>
	</view>

	<view>
		选中商品数量：{{totalNum}}&nbsp;件
	</view>

	<view>
		选中商品总价：{{totalPrice}}&nbsp;元
	</view>

	<view>-----------------------</view>

	<view v-for="item in goods" :key="item.id">
		<view>{{item}}</view>
	</view>
</template>

<script setup>
	import {
		computed,
		ref
	} from 'vue'

	const goods = ref([{
			id: "11",
			name: "小米",
			price: 4999,
			checked: false
		},
		{
			id: "22",
			name: "华为",
			price: 6899,
			checked: false
		},
		{
			id: "33",
			name: "oppo",
			price: 2988,
			checked: false
		},
		{
			id: "44",
			name: "苹果",
			price: 9888,
			checked: false
		},
	]);

	const goodValues = goods.value;

	// 删除商品项
	function remove(index) {
		goodValues.splice(index, 1);
	}

	// 计算选中商品数量
	const selectedItems = ref([]);

	function onChange(e) {
		selectedItems.value = e.detail.value;

		goodValues.forEach(item =>
			item.checked = e.detail.value.includes(item.id));
	}

	const totalNum = computed(() => selectedItems.value.length);

	// 计算选中商品总价
	let totalPrice = computed(() => goodValues.filter(item => item.checked).reduce((prev, current) => prev + current.price,
		0));
</script>

<style lang="scss" scoped>
	.list {
		padding: 10px;

		.item {
			padding: 10px 0px;

			.remove {
				color: red;
				padding-left: 30px;
			}

			.price {
				padding-left: 30px;
			}
		}
	}
</style>