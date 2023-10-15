//#define SQL非查询语句
//#define 执行实体类SQL查询语句
//#define 执行任意SQL查询语句

using Lib;
using Microsoft.EntityFrameworkCore;

using TestDBContext ctx = new TestDBContext();

#if SQL非查询语句

Console.WriteLine("请输入最低价格");
double price = double.Parse(Console.ReadLine());

Console.WriteLine("请输入姓名");
string aName = Console.ReadLine();

////ExecuteSqlInterpolatedAsync(推荐使用,自动处理查询参数)
//int rows = await ctx.Database.ExecuteSqlInterpolatedAsync(@$"
//     insert into T_Books (Title,PubTime,Price,AuthorName)
//     select Title, PubTime, Price,{aName} from T_Books where Price>{price}");

//ExecuteSqlRawAsync(不推荐使用,需要自己处理查询参数)
int rows = await ctx.Database.ExecuteSqlRawAsync(@"
     insert into T_Books (Title,PubTime,Price,AuthorName)
     select Title, PubTime, Price,{0} from T_Books where Price>{1}",
     aName, price);

Console.WriteLine("影响行数：" + rows);
#elif 执行实体类SQL查询语句
int year = int.Parse(Console.ReadLine());
IQueryable<Book> books = ctx.Books.FromSqlInterpolated(@$"select * from T_Books
        where DatePart(year,PubTime)>{year}");
foreach (Book b in books.Skip(3).Take(6))
{
    Console.WriteLine(b.Title);
}
/*
 * FromSqlInterpolated的使用有如下局限性：
 *  1.Sql查询必须返回实体类型对应数据库表的所有列
 *  2.查询结果集中的列名必须与属性映射到的列名匹配
 *  3.Sql语句只能进行单表查询，不能使用join语句进行关联查询，
 *      但是可以在查询后使用Include方法进行关联数据的获取
 */
#elif 执行任意SQL查询语句
var conn = ctx.Database.GetDbConnection();
if (conn.State != System.Data.ConnectionState.Open)
{
    conn.Open();
}

using (var cmd = conn.CreateCommand())
{
    cmd.CommandText = "select * from T_Books where Id=@Id";

    var p1 = cmd.CreateParameter();
    p1.ParameterName = "@Id";
    p1.Value = 1;

    cmd.Parameters.Add(p1);
    using (var reader = cmd.ExecuteReader())
    {
        while (reader.Read())
        {
            Console.WriteLine(reader["Title"].ToString());
            Console.WriteLine(reader["Price"].ToString());
        }
    }
}
#endif