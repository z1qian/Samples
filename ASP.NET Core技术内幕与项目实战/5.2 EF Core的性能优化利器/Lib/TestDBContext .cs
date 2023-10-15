using Microsoft.EntityFrameworkCore;
namespace Lib;

public class TestDBContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    public DbSet<House> Houses { get; set; }

    /// <summary>
    /// 程序要连接的数据库进行配置
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connStr = "Server=.\\SQLExpress;DataBase=DemoDB;Trusted_Connection=True;trustServerCertificate=true;Application Name=TestConsoleApp";
        optionsBuilder.UseSqlServer(connStr);
        optionsBuilder.UseBatchEF_MSSQL();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
