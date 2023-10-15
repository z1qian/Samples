using Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static System.Reflection.Metadata.BlobBuilder;

using TestDBContext ctx = new TestDBContext();

////使用AsNoTracking查询出来的实体类是不被上下文跟踪的
//Book[] books = ctx.Books.AsNoTracking().Take(3).ToArray();

//Book b1 = books[0];
//b1.Title = "abc";
//EntityEntry entry1 = ctx.Entry(b1);
//Console.WriteLine(entry1.State);

////通过两条SQL语句完成数据的更新
//Book b1 = ctx.Books.Single(b => b.Id == 10);
//b1.Title = "yzk";
//ctx.SaveChanges();

////通过一条SQL语句完成数据的更新
//Book b1 = new Book { Id = 10 };
//b1.Title = "yzk1";

//var entry1 = ctx.Entry(b1);
//entry1.Property("Title").IsModified = true;
//Console.WriteLine(entry1.DebugView.LongView);

//ctx.SaveChanges();

////用一条语句删除数据
//Book b1 = new Book { Id = 28 };
//ctx.Entry(b1).State = EntityState.Deleted;
//ctx.SaveChanges();

////Find和FindAsync方法
//var book = await ctx.Books.FindAsync(1L);
//Console.WriteLine(book.Title);

////直接返回在上下文中被跟踪的对象，并不会去数据库中查询
//var book2 = await ctx.Books.FindAsync(1L);
//Console.WriteLine(book2.Title);

////测试EF Core是否支持批量操作呢
//ctx.RemoveRange(ctx.Books.Where(b => b.Id >= 24));
//ctx.SaveChanges();

////使用Zack.EFCore.Batch.MSSQL_NET6批量删除数据
//await ctx.DeleteRangeAsync<Book>(b => b.Id >= 22);

//全局查询筛选器
int count = ctx.Books.Where(b => b.Price > 0).Count();
Console.WriteLine(count);

//临时忽略全局查询筛选器
int allCount = ctx.Books
    .IgnoreQueryFilters()
    .Where(b => b.Price > 0).Count();
Console.WriteLine("共有数据" + allCount + "条");