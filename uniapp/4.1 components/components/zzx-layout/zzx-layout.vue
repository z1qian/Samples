<template>
	<view class="layout">
		<view class="header">
			<slot name="header"></slot>
		</view>

		<view class="main">
			<slot name="main"></slot>
		</view>

		<view class="fotter">fotter区域</view>
	</view>
</template>

<script setup></script>

<style lang="scss" scoped>
.header {
	height: 100px;
	background-color: #cfcfcf;
}

.main {
	min-height: 200px;
}

.fotter {
	height: 120px;
	background-color: orange;
}
</style>
