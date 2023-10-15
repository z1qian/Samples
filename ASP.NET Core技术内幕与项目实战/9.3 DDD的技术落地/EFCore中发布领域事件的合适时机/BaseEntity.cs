using MediatR;

namespace EFCore中发布领域事件的合适时机;

public abstract class BaseEntity : IDomainEvents
{
    private readonly List<INotification> DomainEvents = new();
    public void AddDomainEvent(INotification eventItem)
    {
        DomainEvents.Add(eventItem);
    }
    public void AddDomainEventIfAbsent(INotification eventItem)
    {
        if (!DomainEvents.Contains(eventItem))
            DomainEvents.Add(eventItem);
    }
    public void ClearDomainEvents()
    {
        DomainEvents.Clear();
    }
    public IEnumerable<INotification> GetDomainEvents()
    {
        return DomainEvents;
    }
}