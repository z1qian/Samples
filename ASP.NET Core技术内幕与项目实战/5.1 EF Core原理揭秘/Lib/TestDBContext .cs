using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace Lib;

public class TestDBContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    /// <summary>
    /// 程序要连接的数据库进行配置
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#pragma warning disable EF1001
        optionsBuilder.ReplaceService<IQueryTranslationPostprocessorFactory,
            SqlServer2008QueryTranslationPostprocessorFactory>();
#pragma warning restore EF1001
        string connStr = "Server=.\\SQLExpress;DataBase=DemoDB;Trusted_Connection=True;trustServerCertificate=true;Application Name=TestConsoleApp";
        optionsBuilder.UseSqlServer(connStr);
    }
}
