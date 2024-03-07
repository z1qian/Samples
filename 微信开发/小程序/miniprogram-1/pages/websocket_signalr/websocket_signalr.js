// pages/websocket_signalr/websocket_signalr.js
const signalR = require("../../utils/signalr.1.0.js");
const senparcWebsocket = require("../../utils/senparc.websocket.2.0.js");

var connection = null;// Signalr 连接
const app = getApp()
var socketOpen = false;//WebSocket 打开状态
Page({

  /**
   * 页面的初始数据
   */
  data: {
    messageTip: '正在连接中，请等待...',
    messageTextArr: [],
    messageContent: '我为人人',
  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow() {
    var that = this;
    const hubUrl = wx.getStorageSync('wssDomainName') + '/SenparcHub';
    console.log(hubUrl);
    const onStart = function () {
      console.log('ws started');
      socketOpen = true;
      that.setData({
        messageTip: 'WebSocket 连接成功！'
      });
    }

    connection = senparcWebsocket.buildConnectionAndStart(hubUrl, signalR, onStart);
    //定义收到消息后触发的事件
    const onReceive = function (res) {
      console.log('收到服务器内容：' + res);
      const jsonResult = JSON.parse(res);
      const currentIndex = that.data.messageTextArr.length + 1;
      const newArr = that.data.messageTextArr;
      newArr.unshift({
        index: currentIndex,
        content: jsonResult.content,
        time: jsonResult.time
      });

      console.log(that.data);
      that.setData({
        messageTextArr: newArr
      });
    }

    senparcWebsocket.onReceiveMessage(onReceive);

    //WebSocket 连接成功
    wx.onSocketOpen(function (res) {
      console.log('WebSocket 连接成功！')
    })
    //WebSocket 已关闭
    wx.onSocketClose(function (res) {
      console.log('WebSocket 已关闭！')
      socketOpen = false;
    })
    //WebSocket 打开失败
    wx.onSocketError(function (res) {
      console.log('WebSocket连接打开失败，请检查！')
    })
  },
  //sendMessage
  formSubmit: function (e) {
    const that = this;
    console.log('formSubmit', e);
    if (socketOpen) {
      const text = e.detail.value.messageContent;//必填，获得输入文字
      const sessionId = wx.getStorageSync("sessionId");//选填，不需要可输入''
      const formId = ''//选填formId用于发送模板消息，不需要可输入''
      senparcWebsocket.sendMessage(text, sessionId, formId);//发送 websocket 请求

      that.setData({
        messageContent: ''//清空文本框内容
      })
    }
    else {
      that.setData({
        messageTip: 'WebSocket 链接失败，请重新连接！'
      });
    }
  }
})