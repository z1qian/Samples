export function compareTime(timestamp) {
	const now = Date.now();
	const diff = now - timestamp;

	if (diff < 60000) {
		return '1分钟内';
	} else if (diff < 3600000) {
		const minutes = Math.floor(diff / 60000);
		return `${minutes}分钟前`;
	} else if (diff < 86400000) {
		const hours = Math.floor(diff / 3600000);
		return `${hours}小时前`;
	} else if (diff < 2592000000) {
		const days = Math.floor(diff / 86400000);
		return `${days}天前`;
	} else if (diff < 7776000000) {
		const months = Math.floor(diff / 2592000000);
		return `${months}月前`;
	} else {
		return null;
	}
}

export function goHome() {
	uni.showModal({
		title: "参数异常",
		content: "参数异常，即将返回主页",
		showCancel: false,
		success() {
			uni.reLaunch({
				url: "/pages/index/index"
			});
		}
	});
}