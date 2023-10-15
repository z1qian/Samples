using MediatR;

namespace MediaEncoder.Domain.EventDatas;
public record EncodingItemStartedEventData(Guid Id, string SourceSystem) : INotification;