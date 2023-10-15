using Listening.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zack.Infrastructure.EFCore;

namespace Listening.Infrastructure;

public class ListeningDbContext : BaseDbContext
{
    /// <summary>
    /// 分类
    /// </summary>
    public DbSet<Category> Categories { get; private set; }//不要忘了写set，否则拿到的DbContext的Categories为null
    /// <summary>
    /// 专辑
    /// </summary>
    public DbSet<Album> Albums { get; private set; }
    /// <summary>
    /// 音频
    /// </summary>
    public DbSet<Episode> Episodes { get; private set; }

    public ListeningDbContext(DbContextOptions options, IMediator mediator) : base(options, mediator)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        modelBuilder.EnableSoftDeletionGlobalFilter();
    }
}
