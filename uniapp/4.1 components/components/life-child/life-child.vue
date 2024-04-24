<template>
	<view>子组件</view>
</template>

<script setup>
import { onUnmounted, onMounted } from 'vue';
onMounted(() => console.log('子组件已挂载完毕'));
onUnmounted(() => console.log('子组件已被卸载'));
</script>

<style lang="scss"></style>
