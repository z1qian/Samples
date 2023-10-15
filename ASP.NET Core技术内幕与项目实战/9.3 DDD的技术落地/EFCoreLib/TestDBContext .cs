using EFCore中实现值对象;
using EFCore中实现充血模型;
using Microsoft.EntityFrameworkCore;

namespace EFCoreLib;

public class TestDBContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Region> Region { get; set; }

    /// <summary>
    /// 程序要连接的数据库进行配置
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connStr = "Server=.\\SQLExpress;DataBase=DemoDB;Trusted_Connection=True;trustServerCertificate=true";
        optionsBuilder.UseSqlServer(connStr);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
