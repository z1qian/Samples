using Microsoft.EntityFrameworkCore;

namespace _4._5_查看EF_Core生成的SQL语句;

internal class TestDBContext:DbContext
{
    public DbSet<Book> Books { get; set; }

    /// <summary>
    /// 程序要连接的数据库进行配置
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connStr = "Server=.\\SQLExpress;DataBase=DemoDB;Trusted_Connection=True;trustServerCertificate=true;";
        optionsBuilder.UseSqlServer(connStr);

        //简单日志输出
        optionsBuilder.LogTo(Console.WriteLine);
    }
}
