using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EFCore中发布领域事件的合适时机;

public abstract class BaseDbContext : DbContext
{
    private readonly IMediator _mediator;
    public BaseDbContext(DbContextOptions options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        throw new NotImplementedException("Don't call SaveChanges");
    }
    public async override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        var domainEntities = this.ChangeTracker.Entries<IDomainEvents>()
            .Where(x => x.Entity.GetDomainEvents().Any());

        var domainEvents = domainEntities.SelectMany(x => x.Entity.GetDomainEvents()).ToList();

        domainEntities.ToList().ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }

        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
