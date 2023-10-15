using MediaEncoder.Domain.Entities;
using MediatR;

namespace MediaEncoder.Domain.EventDatas;
public record EncodingItemCreatedEventData(EncodingItem Value) : INotification;