<!-- 以下代码只适合h5页面，因为div，h1，h2，span等只在h5中适配，它们本来就是h5中的标签，在小程序和app中不适配 -->
<template>
	<div class="box">
		<h1>
			uniapp<span class="row">零基础入门</span>
		</h1>
		<view>从基础到进阶</view>
		<h2>标题二加粗<i>标签</i></h2>
		<view>
			<navigator url="/pages/medium/medium">跳转至medium页</navigator>
		</view>
	</div>
</template>

<script>
	export default {}
</script>

<style lang="scss">
	.box {
		// width: 100px;
		height: 100px;
		background-color: pink;

		h1 {
			font-size: 40px;
			color: red;

			.row {
				font-style: italic;
			}
		}
	}
</style>