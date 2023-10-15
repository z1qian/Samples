using Microsoft.EntityFrameworkCore;

namespace 主键类型的配置;

internal class TestDBContext : DbContext
{
    public DbSet<Author> Authors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }

    /// <summary>
    /// 程序要连接的数据库进行配置
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connStr = "Server=.\\SQLExpress;DataBase=DemoDB;Trusted_Connection=True;trustServerCertificate=true;";
        optionsBuilder.UseSqlServer(connStr);
    }
}
