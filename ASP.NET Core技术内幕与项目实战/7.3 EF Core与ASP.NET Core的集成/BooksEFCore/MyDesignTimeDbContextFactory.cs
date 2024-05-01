using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics;

namespace BooksEFCore;
/*
 * 当项目中存在一个IDesignTimeDbContextFactory接口的实现类的时候，
 * 数据库迁移工具就会调用这个实现类的CreateDbContext方法来获取上下文对象，
 * 然后迁移工具会使用这个上下文对象来连接数据库。
 */

internal class MyDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
{
    public MyDbContext CreateDbContext(string[] args)
    {
        //#if DEBUG
        //        Debugger.Launch();
        //#endif
        DbContextOptionsBuilder<MyDbContext> builder = new();

        string connStr = Environment.GetEnvironmentVariable("ConnectionStrings:BooksEFCore")
            ?? "没有获取到连接字符串！";
        Console.WriteLine(connStr);

        builder.UseSqlServer(connStr);

        return new MyDbContext(builder.Options);
    }
}