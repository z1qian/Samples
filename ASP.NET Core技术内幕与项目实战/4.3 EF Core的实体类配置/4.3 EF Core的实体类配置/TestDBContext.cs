using Microsoft.EntityFrameworkCore;

namespace _4._3_EF_Core的实体类配置;

internal class TestDBContext : DbContext
{
    public DbSet<Student> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //视图与实体类映射
        modelBuilder.Entity<Student>().ToView("StudentView");

        //排除属性映射
        modelBuilder.Entity<Student>().Ignore(s => s.Address);

        //设置对应数据库表中的列名
        modelBuilder.Entity<Student>()
            .Property(s => s.Id)
            .HasColumnName("StudentId");

        //为列指定数据类型
        modelBuilder.Entity<Student>()
            .Property(s => s.Name)
            .HasColumnType("varchar(200)");

        //EF Core默认把名字为Id或者“实体类型+Id”的属性作为主键
        //手动配置主键
        //推荐使用"Id"作为主键名称
        modelBuilder.Entity<Student>().HasKey(s => s.Id);

        //配置索引
        modelBuilder.Entity<Student>()
            .HasIndex(s => s.Name)
            //唯一索引
            .IsUnique()
            //聚集索引
            .IsClustered();

        //配置复合索引
        modelBuilder.Entity<Student>().HasIndex(s => new
        {
            s.Name,
            s.Address
        });

        //重载方法
        modelBuilder.Entity<Student>()
            //.Property(s => s.Id)
            .Property("Id")
            .HasColumnName("StudentId");

        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
