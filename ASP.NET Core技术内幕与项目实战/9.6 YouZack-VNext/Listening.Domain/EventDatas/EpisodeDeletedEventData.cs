using MediatR;

namespace Listening.Domain.EventDatas;

public record EpisodeDeletedEventData(Guid Id) : INotification;