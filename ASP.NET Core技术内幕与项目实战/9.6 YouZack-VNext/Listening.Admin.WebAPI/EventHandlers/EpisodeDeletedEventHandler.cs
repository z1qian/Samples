using Listening.Domain.EventDatas;
using MediatR;
using Zack.EventBus;

namespace Listening.Admin.WebAPI.EventHandlers;

public class EpisodeDeletedEventHandler : INotificationHandler<EpisodeDeletedEventData>
{
    private readonly IEventBus eventBus;

    public EpisodeDeletedEventHandler(IEventBus eventBus)
    {
        this.eventBus = eventBus;
    }

    public Task Handle(EpisodeDeletedEventData notification, CancellationToken cancellationToken)
    {
        var id = notification.Id;
        eventBus.Publish("ListeningEpisode.Deleted", new { Id = id });
        return Task.CompletedTask;
    }
}