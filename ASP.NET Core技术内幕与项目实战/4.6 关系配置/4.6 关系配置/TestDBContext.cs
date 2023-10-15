using Microsoft.EntityFrameworkCore;

namespace _4._6_关系配置;

internal class TestDBContext : DbContext
{
    public DbSet<Article> Articles { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Leave> Leaves { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }

    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }

    /// <summary>
    /// 程序要连接的数据库进行配置
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connStr = "Server=.\\SQLExpress;DataBase=DemoDB;Trusted_Connection=True;trustServerCertificate=true;";
        optionsBuilder.UseSqlServer(connStr);
        //optionsBuilder.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
