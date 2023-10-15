using MediaEncoder.Domain.EventDatas;
using Zack.EventBus;

namespace MediaEncoder.WebAPI.EventHandlers;
class EncodingItemFailedEventHandler : INotificationHandler<EncodingItemFailedEventData>
{
    private readonly IEventBus _eventBus;

    public EncodingItemFailedEventHandler(IEventBus eventBus)
    {
       _eventBus = eventBus;
    }
    public Task Handle(EncodingItemFailedEventData notification, CancellationToken cancellationToken)
    {
        _eventBus.Publish("MediaEncoding.Failed", notification);
        return Task.CompletedTask;
    }
}