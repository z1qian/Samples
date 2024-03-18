<template>
	<view>
		<view>
			<video src="/static/medium/dd48ef4d0a0cbfc432ca832029e155dd.mp4" muted initial-time=2 :danmu-btn="false"
				show-mute-btn></video>
		</view>
		<view>
			<audio src="https://web-ext-storage.dcloud.net.cn/uni-app/ForElise.mp3" controls author="uni-app"
				name="uni-app的音频" poster="https://qiniu-web-assets.dcloud.net.cn/unidoc/zh/music-a.png"></audio>
		</view>

		<view>
			<button size="mini" type="primary">这是一个primary按钮</button>
			<button size="mini" type="warn">这是一个warn按钮</button>
			<button size="mini" type="default">这是一个default按钮</button>
			<button size="mini" plain>这是一个plain按钮</button>
			<button size="mini" disabled>这是一个disabled按钮</button>
			<button size="mini" loading>这是一个loading按钮</button>
		</view>
		
		<view>
			<input type="text" value="123" maxlength="10"/>
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			return {

			}
		},
		methods: {

		}
	}
</script>

<style>

</style>