using MediaEncoder.Domain.EventDatas;
using Zack.EventBus;

namespace MediaEncoder.WebAPI.EventHandlers;
class EncodingItemStartedEventHandler : INotificationHandler<EncodingItemStartedEventData>
{
    private readonly IEventBus _eventBus;

    public EncodingItemStartedEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }
    public Task Handle(EncodingItemStartedEventData notification, CancellationToken cancellationToken)
    {
        _eventBus.Publish("MediaEncoding.Started", notification);
        return Task.CompletedTask;
    }
}