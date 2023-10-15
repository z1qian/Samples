using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using 标识框架的使用.Models;

namespace 标识框架的使用;

public class IdDbContext : IdentityDbContext<User, Role, long>
{
    public IdDbContext(DbContextOptions<IdDbContext> options)
       : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}