using MediatR;

namespace MediaEncoder.Domain.EventDatas;
public record EncodingItemFailedEventData(Guid Id, string SourceSystem, string ErrorMessage) : INotification;