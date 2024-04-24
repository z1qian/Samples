const SYSTEM_INFO = uni.getSystemInfoSync();

// 获取状态栏高度
export const getStatusBarHeight = () => SYSTEM_INFO.statusBarHeight ||
	uni.upx2px(30);

// 获取胶囊按钮高度
export const getTitleBarHeight = () => {
	if (uni.getMenuButtonBoundingClientRect) {
		const {
			top,
			height
		} = uni.getMenuButtonBoundingClientRect();
		return (top - getStatusBarHeight()) * 2 + height;
	}
	return uni.upx2px(80)
}

// 填充区高度
export const getNavBarHeight = () => getStatusBarHeight() + getTitleBarHeight();

// 获取抖音小程序图标信息
export const getLeftIconLeft = () => {
	// #ifdef MP-TOUTIAO
	const {
		leftIcon: {
			left,
			width
		}
	} = tt.getCustomButtonBoundingClientRect();
	return left + parseInt(width);
	// #endif

	return 0;
}