using IdentityService.Domain;
using Zack.EventBus;

namespace IdentityService.WebAPI.Events;

[EventName("IdentityService.UserAdmin.Created")]
public class UserCreatedEventHandler : JsonIntegrationEventHandler<UserCreatedEventData>
{
    private readonly ISmsSender _smsSender;

    public UserCreatedEventHandler(ISmsSender smsSender)
    {
        _smsSender = smsSender;
    }

    public override Task HandleJson(string eventName, UserCreatedEventData? eventData)
    {
        if (eventData == null)
        {
            return Task.CompletedTask;
        }
        //发送初始密码给被创建用户的手机
        return _smsSender.SendAsync(eventData.PhoneNum, eventData.Password);
    }
}
