import {
	request
} from '/utils/request.js';

// 获取轮播图
export function apiGetBanner() {
	return request({
		url: "/homeBanner"
	});
}

// 获取每日推荐
export function apiGetDailyRecommend() {
	return request({
		url: "/randomWall"
	});
}

// 获取公告
export function apiGetNoticeList(data = {}) {
	return request({
		url: "/wallNewsList",
		data
	});
}

// 获取专题精选
export function apiGetClassifyList(data = {}) {
	return request({
		url: "/classify",
		data
	});
}


// 获取分类下的壁纸列表
export function apiGetWallList(data = {}) {
	return request({
		url: "/wallList",
		data
	});
}

// 壁纸评分
export function apiSetupScore(data = {}) {
	return request({
		url: "/setupScore",
		data
	});
}

// 壁纸下载
export function apiDownloadWall(data = {}) {
	return request({
		url: "/downloadWall",
		data
	});
}

// 获取单个壁纸详情
export function apiGetDetailWall(data = {}) {
	return request({
		url: "/detailWall",
		data
	});
}

// 获取用户个人信息
export function apiGetUserInfo(data = {}) {
	return request({
		url: "/userInfo",
		data
	});
}

// 获取我的评分/下载列表
export function apiGetUserWallList(data = {}) {
	return request({
		url: "/userWallList",
		data
	});
}

// 获取壁纸资讯公告详情
export function apiGetwallNewsDetail(data = {}) {
	return request({
		url: "/wallNewsDetail",
		data
	});
}

// 搜索壁纸
export function apiSearchWall(data = {}) {
	return request({
		url: "/searchWall",
		data
	});
}