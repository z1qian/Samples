using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;

namespace Users.Infrastructure;

public class UserDbContext : DbContext
{
    /*
     * 只为User、UserLoginHistory两个聚合根实体类声明DbSet属性，而不为User聚合中的UserAccessFail实体类定义DbSet属性，
     * 这样就约束开发人员尽量通过聚合根来操作聚合内的实体类
     */
    public DbSet<User> Users { get; private set; }
    public DbSet<UserLoginHistory> LoginHistories { get; private set; }

    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
