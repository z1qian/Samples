using Microsoft.EntityFrameworkCore;

namespace _4.解决JWT无法提前撤回的难题;

internal class TestDBContext : DbContext
{
    public TestDBContext(DbContextOptions<TestDBContext> options)
      : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
