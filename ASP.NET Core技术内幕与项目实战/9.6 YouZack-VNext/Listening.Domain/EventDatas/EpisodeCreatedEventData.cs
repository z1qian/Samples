using Listening.Domain.Entities;
using MediatR;

namespace Listening.Domain.EventDatas;

public record EpisodeCreatedEventData(Episode Value) : INotification;