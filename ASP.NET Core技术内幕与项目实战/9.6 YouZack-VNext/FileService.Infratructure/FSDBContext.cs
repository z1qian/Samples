using FileService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zack.Infrastructure.EFCore;

namespace FileService.Infratructure;

public class FSDBContext : BaseDbContext
{
    public DbSet<UploadedItem> UploadItems { get; private set; }

    public FSDBContext(DbContextOptions<FSDBContext> options, IMediator mediator)
        : base(options, mediator)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
