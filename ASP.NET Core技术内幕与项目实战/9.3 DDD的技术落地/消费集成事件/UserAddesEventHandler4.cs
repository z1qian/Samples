using Zack.EventBus;

namespace 消费集成事件
{
    [EventName("UserAdded")]
    public class UserAddesEventHandler4 : JsonIntegrationEventHandler<UserData>
    {
        private readonly ILogger<UserAddesEventHandler4> logger;
        public UserAddesEventHandler4(ILogger<UserAddesEventHandler4> logger)
        {
            this.logger = logger;
        }
        public override Task HandleJson(string eventName, UserData eventData)
        {
            logger.LogInformation("第四位处理者");
            return Task.CompletedTask;
        }
    }
}
