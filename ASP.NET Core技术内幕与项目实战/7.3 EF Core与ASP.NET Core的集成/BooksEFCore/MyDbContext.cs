﻿using Microsoft.EntityFrameworkCore;

namespace BooksEFCore;

public class MyDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    public MyDbContext(/*数据库连接配置对象*/DbContextOptions<MyDbContext> options) : base(options)
    { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}