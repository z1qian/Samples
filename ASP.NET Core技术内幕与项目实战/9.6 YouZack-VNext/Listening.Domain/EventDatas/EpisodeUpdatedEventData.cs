using Listening.Domain.Entities;
using MediatR;

namespace Listening.Domain.EventDatas;

public record EpisodeUpdatedEventData(Episode Value):INotification;