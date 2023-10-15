using MediaEncoder.Domain.EventDatas;
using Zack.EventBus;

namespace MediaEncoder.WebAPI.EventHandlers;
class EncodingItemCreatedEventHandler : INotificationHandler<EncodingItemCreatedEventData>
{
    private readonly IEventBus _eventBus;

    public EncodingItemCreatedEventHandler(IEventBus eventBus)
    {
       _eventBus = eventBus;
    }

    public Task Handle(EncodingItemCreatedEventData notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}