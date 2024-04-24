<template>
	<view class="">
		<view v-for="(item,index) in 10" :key="index">
			我是 item : {{item}} - index : {{index}}
		</view>

		<view class="box"></view>

		<view v-for="(item,index) in nba" :key="item.id">
			姓名：{{item.name}} , 球衣：{{item.num}}
		</view>
		
		<view class="box"></view>
		
		<view>
			<ul>
			  <template v-for="item in nba" :key="item.id">
			    <li>{{ item.name }}</li>
			  </template>
			</ul>
		</view>
	</view>
</template>

<script setup>
	let nba = [{
			id: 1,
			name: '姚明',
			num: 21
		},
		{
			id: 2,
			name: '詹姆斯',
			num: 11
		},
		{
			id: 3,
			name: '乔丹',
			num: 23
		},
	];
</script>

<style lang="scss" scoped>

.box{
	margin: 20px 0px;
}
</style>