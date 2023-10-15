using IdentityService.Domain;
using Zack.EventBus;

namespace IdentityService.WebAPI.Events;

[EventName("IdentityService.UserAdmin.PasswordReset")]
public class ResetPasswordEventHandler : JsonIntegrationEventHandler<ResetPasswordEventData>
{
    private readonly ISmsSender _smsSender;

    public ResetPasswordEventHandler(ISmsSender smsSender)
    {
        _smsSender = smsSender;
    }

    public override Task HandleJson(string eventName, ResetPasswordEventData? eventData)
    {
        if (eventData == null)
        {
            return Task.CompletedTask;
        }

        //发送密码给被用户的手机
        return _smsSender.SendAsync(eventData.PhoneNum, eventData.Password);
    }
}
