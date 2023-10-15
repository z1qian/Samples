using Zack.EventBus;

namespace 消费集成事件;

//我们可以在一个类上添加多个[EventName]，以便让类监听多个事件
[EventName("UserAdded")]
public class UserAddesEventHandler : IIntegrationEventHandler
{
    private readonly ILogger<UserAddesEventHandler> logger;
    public UserAddesEventHandler(ILogger<UserAddesEventHandler> logger)
    {
        this.logger = logger;
    }
    public Task Handle(string eventName, string eventData)
    {
        logger.LogInformation($"新建了用户:eventName:{eventName},eventData:{eventData}");
        return Task.CompletedTask;
    }
}