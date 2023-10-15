using Lib;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using TestDBContext ctx = new TestDBContext();

//Book[] books = ctx.Books.Take(3).ToArray();
//Book b1 = books[0];
//Book b2 = books[1];
//Book b3 = books[2];
//Book b4 = new Book { Title = "零基础趣学C语言", AuthorName = "杨中科" };
//Book b5 = new Book { Title = "百年孤独", AuthorName = "马尔克斯" };

//b1.Title = "abc";

//ctx.Remove(b3);

//ctx.Add(b4);

//EntityEntry entry1 = ctx.Entry(b1);
//EntityEntry entry2 = ctx.Entry(b2);
//EntityEntry entry3 = ctx.Entry(b3);
//EntityEntry entry4 = ctx.Entry(b4);
//EntityEntry entry5 = ctx.Entry(b5);

//Console.WriteLine("b1.State:" + entry1.State);
//Console.WriteLine("b1.DebugView:" + entry1.DebugView.LongView);
//Console.WriteLine("b2.State:" + entry2.State);
//Console.WriteLine("b3.State:" + entry3.State);
//Console.WriteLine("b4.State:" + entry4.State);
//Console.WriteLine("b5.State:" + entry5.State);

#region 修改实体的部分属性
var book = ctx.Books.SingleOrDefault(b => b.Id == 1);
if (book != null)
{
    book.Remark = "测试修改部分属性";

    //只修改Remark属性
    await ctx.SaveChangesAsync();
}

#endregion