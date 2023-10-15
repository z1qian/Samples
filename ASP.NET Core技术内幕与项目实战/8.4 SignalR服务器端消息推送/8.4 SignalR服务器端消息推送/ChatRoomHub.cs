using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace _8._4_SignalR服务器端消息推送;

//所有的客户端和服务器端都通过这个集线器进行通信
[Authorize]
public class ChatRoomHub : Hub
{
    //不建议注入TestDBContext来写义务逻辑代码，这会影响SignalR的性能，Hub类中的方法只应该用于消息的发布
    //SignalR中客户端给服务器端传递消息的超时时间为30s，如果对Hub类中的方法的调用执行时间超过30s，程序就会报错。
    //Hub类的生命周期为瞬态
    public ChatRoomHub(TestDBContext ctx)
    {
        Console.WriteLine($"ctx is null ? " + ctx == null);
    }

    //在编写Hub中的方法时，我们一般不设置方法名以Async结尾
    public Task SendPublicMessage(string message)
    {
        //如果我们想实现多个聊天室的效果，就可以把用户放入不同的分组中，这样每个分组就是一个聊天室。

        //string connId = this.Context.ConnectionId;
        //string msg = $"{connId} {DateTime.Now}:{message}";
        //return Clients.All.SendAsync("ReceivePublicMessage", msg);


        string name = this.Context.User!.FindFirst(ClaimTypes.Name)!.Value;
        string msg = $"{name} {DateTime.Now}:{message}";
        return Clients.All.SendAsync("ReceivePublicMessage", msg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="destUserId">私聊目标的Id</param>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task<string> SendPrivateMessage(string destUserId, string message)
    {
        string srcUserName = this.Context.User!.FindFirst(ClaimTypes.Name)!.Value;
        string time = DateTime.Now.ToShortTimeString();

        //将多个参数发送到客户端
        await this.Clients.User(destUserId).SendAsync("ReceivePrivateMessage",
           srcUserName, time, message);

        return "ok";
    }

    public override Task OnConnectedAsync()
    {
        Console.WriteLine(Context.ConnectionId + "已连接");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine(Context.ConnectionId + "已断开");
        return base.OnDisconnectedAsync(exception);
    }
}