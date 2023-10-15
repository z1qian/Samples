using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zack.DomainCommons.Models;

public record BaseEntity : IEntity, IDomainEvents
{

    [NotMapped]
    private readonly List<INotification> _domainEvents = new();

    public Guid Id { get; protected set; } = Guid.NewGuid();

    public void AddDomainEvent(INotification eventItem)
    {
        _domainEvents.Add(eventItem);
    }

    public void AddDomainEventIfAbsent(INotification eventItem)
    {
        if (!_domainEvents.Contains(eventItem))
        {
            _domainEvents.Add(eventItem);
        }
    }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public IEnumerable<INotification> GetDomainEvents()
    {
        return _domainEvents;
    }
}
