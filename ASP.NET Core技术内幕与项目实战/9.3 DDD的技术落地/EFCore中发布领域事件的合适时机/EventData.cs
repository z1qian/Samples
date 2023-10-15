using MediatR;

namespace EFCore中发布领域事件的合适时机;

public record UserAddedEvent(User Item) : INotification;
public record UserUpdatedEvent(Guid Id) : INotification;
public record UserSoftDeletedEvent(Guid Id) : INotification;