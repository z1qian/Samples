using Zack.EventBus;

namespace 消费集成事件
{
    /*
     * 进行微服务开发的时候，为了降低耦合，我们一般不要在消息发布方和消息消费方的项目中引用同样的消息数据类的定义，
     * 因此我们需要在两个项目中分别声明消息数据类的定义。如果读者不想定义额外的消息数据类，
     * 也可以使用DynamicIntegrationEventHandler来把收到的JSON字符串解析为dynamic类型
     */
    [EventName("UserAdded")]
    public class UserAddesEventHandler2 : DynamicIntegrationEventHandler
    {
        private readonly ILogger<UserAddesEventHandler2> logger;
        public UserAddesEventHandler2(ILogger<UserAddesEventHandler2> logger)
        {
            this.logger = logger;
        }
        public override Task HandleDynamic(string eventName, dynamic eventData)
        {
            logger.LogInformation($"Dynamic:{eventData.UserName}");
            return Task.CompletedTask;
        }
    }
}
