using MediatR;

namespace MediaEncoder.Domain.EventDatas;
public record EncodingItemCompletedEventData(Guid Id, string SourceSystem, Uri OutputUrl) : INotification;