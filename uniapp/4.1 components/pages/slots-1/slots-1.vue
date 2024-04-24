<template>
	<view class="out">
		<zzx-layout>
			<template v-slot:header>1的头部</template>

			<template v-slot:main>1的main</template>
		</zzx-layout>
	</view>
</template>

<script setup>
import { ref } from 'vue';
</script>

<style lang="scss" scoped></style>
