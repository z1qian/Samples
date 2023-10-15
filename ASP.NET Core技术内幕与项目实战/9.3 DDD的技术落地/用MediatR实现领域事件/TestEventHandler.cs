using MediatR;

namespace 用MediatR实现领域事件;

//事件的处理者要实现INotificationHandler<TNotification>接口，
//其中的泛型参数TNotification代表此事件处理者要处理的消息类型
//所有TNotification类型的事件都会被事件处理者处理
public class TestEventHandler1 : INotificationHandler<TestEvent>
{
    public Task Handle(TestEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"我收到了{notification.UserName}");
        return Task.CompletedTask;
    }
}
public class TestEventHandler2 : INotificationHandler<TestEvent>
{
    public async Task Handle(TestEvent notification, CancellationToken cancellationToken)
    {
        await File.WriteAllTextAsync("1.txt", $"来了{notification.UserName}");
    }
}