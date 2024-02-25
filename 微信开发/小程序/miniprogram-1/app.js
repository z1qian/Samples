// app.js
App({
  onLaunch() {
    // 展示本地存储能力
    const logs = wx.getStorageSync('logs') || []
    logs.unshift(Date.now())
    wx.setStorageSync('logs', logs)

    //调试状态使用本地服务器，非调试状态使用远程服务器
    const isDebug = false;
    if (!isDebug) {
      //远程域名
      wx.setStorageSync('domainName', "http://coren.frp.senparc.com")
      wx.setStorageSync('wssDomainName', "wss://coren.frp.senparc.com")
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
        const domainName = wx.getStorageSync('domainName');
        wx.request({
          url: `${domainName}/WxOpen/OnLogin`,
          method: "POST",
          // POST 请求传递一个参数，必须设置以下 header，不然后台接收不到 code
          header: { 'content-type': 'application/x-www-form-urlencoded' },
          data: {
            code: res.code
          },
          success: function (res) {
            const { msg,openId,sessionKey,sessionId } = res.data
            wx.showModal({
              title: msg,
              content: `openId:${openId}\r\sessionKey:${sessionKey}`,
            })
            console.log('wx.login - request-/WxOpen/OnLogin Result:', res);
            wx.setStorageSync('sessionId', sessionId);
          }
        })
      }
    })
  },
  globalData: {
    userInfo: null
  }
})
