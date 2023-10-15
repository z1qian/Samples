using Microsoft.EntityFrameworkCore;

namespace 数据的定时导出;

public class TestDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public TestDbContext(DbContextOptions<TestDbContext> options)
       : base(options)
    {
    }
}