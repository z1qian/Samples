// app.js
App({
  onLaunch() {
    // 展示本地存储能力
    const logs = wx.getStorageSync('logs') || []
    logs.unshift(Date.now())
    wx.setStorageSync('logs', logs)

    const isDebug = true;//调试状态使用本地服务器，非调试状态使用远程服务器
    if (!isDebug) {
      //远程域名
      wx.setStorageSync('domainName', "http://szrdtest.frp.senparc.com")
      wx.setStorageSync('wssDomainName', "wss://szrdtest.frp.senparc.com")
    }
    else {
      //本地测试域名
      wx.setStorageSync('domainName', "https://127.0.0.1:44322")
      wx.setStorageSync('wssDomainName', "ws://127.0.0.1:44322")
    }

    // 登录
    wx.login({
      success: res => {
        // 发送 res.code 到后台换取 openId, sessionKey, unionId
      }
    })
  },
  globalData: {
    userInfo: null
  }
})
