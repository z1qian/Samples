using MediaEncoder.Domain.EventDatas;
using Zack.EventBus;

namespace MediaEncoder.WebAPI.EventHandlers;
class EncodingItemCompletedEventHandler : INotificationHandler<EncodingItemCompletedEventData>
{
    private readonly IEventBus _eventBus;

    public EncodingItemCompletedEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public Task Handle(EncodingItemCompletedEventData notification, CancellationToken cancellationToken)
    {
        //把转码任务状态变化的领域事件，转换为集成事件发出
        _eventBus.Publish("MediaEncoding.Completed", notification);
        return Task.CompletedTask;
    }
}