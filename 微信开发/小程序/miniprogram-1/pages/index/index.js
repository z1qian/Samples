// index.js
const defaultAvatarUrl = 'https://mmbiz.qpic.cn/mmbiz/icTdbqWNOwNRna42FI242Lcia07jQodd2FJGIYQfG0LAJGFxM4FbnQP6yfMxBgJ0F3YRqJCJ1aPAK2dQagdusBZg/0'

Page({
  onLoad() {
  },
  data: {
    motto: 'Hello World',
    userInfo: {
      avatarUrl: defaultAvatarUrl,
      nickName: '',
    },
    hasUserInfo: false,
    canIUseGetUserProfile: wx.canIUse('getUserProfile'),
    canIUseNicknameComp: wx.canIUse('input.type.nickname'),
  },
  bindViewTap() {
    wx.navigateTo({
      url: '../logs/logs'
    })
  },
  onChooseAvatar(e) {
    const { avatarUrl } = e.detail
    const { nickName } = this.data.userInfo
    this.setData({
      "userInfo.avatarUrl": avatarUrl,
      hasUserInfo: nickName && avatarUrl && avatarUrl !== defaultAvatarUrl,
    })
  },
  onInputChange(e) {
    const nickName = e.detail.value
    const { avatarUrl } = this.data.userInfo
    this.setData({
      "userInfo.nickName": nickName,
      hasUserInfo: nickName && avatarUrl && avatarUrl !== defaultAvatarUrl,
    })
  },
  getUserProfile(e) {
    // 推荐使用wx.getUserProfile获取用户信息，开发者每次通过该接口获取用户个人信息均需用户确认，开发者妥善保管用户快速填写的头像昵称，避免重复弹窗
    wx.getUserProfile({
      desc: '展示用户信息', // 声明获取用户个人信息后的用途，后续会展示在弹窗中，请谨慎填写
      success: (res) => {
        console.log(res)
        this.setData({
          userInfo: res.userInfo,
          hasUserInfo: true
        })
      }
    })
  },
  doRequest: function () {
    const that = this;
    const domainName = wx.getStorageSync('domainName');
    console.log('domainName：' + domainName);
    wx.request({
      url: `${domainName}/WxOpen/RequestData`,
      data: {
        nickName: that.data.userInfo.nickName
      },
      method: "Get",
      success: function (res) {
        console.log(res);
        var json = res.data;
        //模组对话框
        wx.showModal({
          title: '收到消息',
          content: json.msg,
          complete: (res) => {
            if (res.cancel) {
              console.log('用户点击取消');
            }

            if (res.confirm) {
              console.log('用户点击确定');
            }
          }
        })
      }
    })
  },
  //发送订阅消息
  sendMessageTemplate: function () {
    const templateId = '44V9uWx-cT4C8780LxIl0xWs9VUrzrPzq9S7zB2xgqw';
    wx.requestSubscribeMessage({
      tmplIds: [templateId],
      success(res) {
        console.log('sendMessageTemplate - success：', res);
        var acceptResult = res[templateId];//'accept'、'reject'、'ban'
        wx.showModal({
          title: '您点击了按钮',
          content: '事件类型：' + acceptResult + '\r\n' + '您将在几秒钟之后收到延迟的提示',
          showCancel: false,
          complete: (res) => {
            if (res.confirm) {
              if (acceptResult != 'accept') {
                console.log(acceptResult);
                return;
              }
              // accept
              wx.request({
                url: `${wx.getStorageSync('domainName')}/WxOpen/SubscribeMessage`,
                method: 'POST',
                header: { 'content-type': 'application/x-www-form-urlencoded' },
                data: {
                  sessionId: wx.getStorageSync('sessionId'),
                  templateId: templateId
                },
                success(msgRes) {
                  const { success, msg } = msgRes.data
                  const title = success ? '操作成功！' : '操作失败！';
                  wx.showModal({
                    title: title,
                    content: msg,
                  });
                }
              })
            }
          }
        })
      },
      fail(res) {
        console.log('sendMessageTemplate - fail：', res);
      }
    })
  },
  //微信支付
  wxPay: function () {
    const domainName = wx.getStorageSync('domainName');
    wx.request({
      url: `${domainName}/WxOpen/GetPrepayid`,//注意：必须使用https
      data: {
        sessionId: wx.getStorageSync('sessionId')
      },
      method: 'POST',
      header: { 'content-type': 'application/x-www-form-urlencoded' },
      success: function (res) {
        const json = res.data;
        console.log(json);
        
        if (json.success) {
          wx.showModal({
            title: '得到预支付id',
            content: json.packageStr,
            showCancel: false
          });

          //开始发起微信支付
          const { timeStamp, nonceStr, packageStr, signType, paySign } = json
          wx.requestPayment({
            "timeStamp": timeStamp,
            "nonceStr": nonceStr,
            "package": packageStr,
            "signType": signType,
            "paySign": paySign,
            "success": function (res) {
              wx.showModal({
                title: '支付成功！',
                content: '请在服务器后台的回调地址中进行支付成功确认，不能完全相信UI！',
                showCancel: false
              });
            },
            "fail": function (res) {
              console.log(res);
              wx.showModal({
                title: '支付失败！',
                content: '请检查日志！',
                showCancel: false
              });
            },
            "complete": function (res) {
              wx.showModal({
                title: '支付流程结束！',
                content: '执行 complete()，成功或失败都会执行！',
                showCancel: false
              });
            }
          })
        }
        else {
          wx.showModal({
            title: '微信支付发生异常',
            content: json.msg,
            showCancel: false
          });
        }
      }
    })
  },
  //WebSocket
  bindWebsocketTap:function(){
    wx.navigateTo({
      url: '../websocket_signalr/websocket_signalr'
    })
  }
})
