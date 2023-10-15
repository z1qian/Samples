//#define 添加
//#define 关联数据的获取
//#define Comment对象添加Article
//#define 获取外键的值
//#define 单向导航属性
//#define 关系配置在哪个实体类中
//#define 一对一
//#define 多对多
//#define 基于关系的复杂查询

using _4._6_关系配置;
using _4._6_关系配置.Migrations;
using Microsoft.EntityFrameworkCore;

using TestDBContext ctx = new TestDBContext();
#if 添加
Article a1 = new Article();
a1.Title = "微软发布.NET 6大版本的首个预览";
a1.Content = "微软昨日在一篇官网博客中宣布了 .NET 6 首个预览版本的到来。";

Comment c1 = new Comment() { Message = "支持" };
Comment c2 = new Comment() { Message = "微软太牛了" };
Comment c3 = new Comment() { Message = "支持！" };

a1.Comments!.Add(c1);
a1.Comments.Add(c2);
a1.Comments.Add(c3);

//并没有显示的添加Comment对象到数据库中
//但Comment对象也被添加到[T_Comments]表中
//因为我们的关系配置可以让EF Core自动完成这些工作
ctx.Articles.Add(a1);
await ctx.SaveChangesAsync();

#elif 关联数据的获取
Article a = ctx.Articles
    .Include(a => a.Comments)
    .AsSplitQuery()
    .Single(a => a.Id == 2);
Console.WriteLine(a.Title);
foreach (Comment c in a.Comments)
{
    Console.WriteLine(c.Id + ":" + c.Message);
}

Console.WriteLine("关联数据的获取完成");
#elif Comment对象添加Article
Article a1 = new Article();
a1.Title = "关于.NET 5正式发布，你应该了解的5件事";
a1.Content = ".NET 5 是.NET Core 3.1 和 .NET Framework 4.8 的后续产品。";

Comment c1 = new Comment() { Message = "已经在用了", Article = a1 };
Comment c2 = new Comment() { Message = "我们公司项目已经升级到.NET 5了", Article = a1 };

ctx.Comments.Add(c1);
ctx.Comments.Add(c2);
await ctx.SaveChangesAsync();

Console.WriteLine("Comment对象添加Article完成");

#elif 获取外键的值

#region inner join方法

//foreach (Comment c in ctx.Comments.Include(c => c.Article))
//  {
//     Console.WriteLine(c.Id + ":" + c.Message + "; " + c.Article!.Id);
//  }
#endregion

#region 外键列方法

foreach (var c in ctx.Comments)
{
    Console.WriteLine($"{c.Id}:{c.Message};{c.ArticleId}");
}
#endregion
Console.WriteLine("获取外键的值结束");
#elif 单向导航属性

#region 添加

//User u1 = new User { Name = "杨中科" };

//Leave leave1 = new Leave();
//leave1.Requester = u1;
//leave1.From = new DateTime(2021, 8, 8);
//leave1.To = new DateTime(2021, 8, 9);
//leave1.Remarks = "阳了，回新西兰";
//leave1.Status = 0;

//ctx.Users.Add(u1);
//ctx.Leaves.Add(leave1);
//await ctx.SaveChangesAsync();

//Console.WriteLine("杨中科请假单已申请");
#endregion

#region 查询

//User u = await ctx.Users.SingleAsync(u => u.Name == "杨中科");
//foreach (var l in ctx.Leaves.Where(l => l.Requester == u))
//{
//    Console.WriteLine(l.Remarks);
//}
#endregion
#elif 关系配置在哪个实体类中
/*
 * 对于单向导航属性，我们只能把关系配置到一方。
 * 因此，
 * 考虑到有单向导航属性的可能，我们一般都用HasOne(…).WithMany(…)这样的方式进行配置
 * 也就是配置在 多 这一端
 */
#elif 一对一
Order order = new Order();
order.Address = "某某市某某区";
order.Name = "USB充电器";

Delivery delivery = new Delivery();
delivery.CompanyName = "蜗牛快递";
delivery.Number = "SN333322888";
delivery.Order = order;

ctx.Deliveries.Add(delivery);
await ctx.SaveChangesAsync();

Order order1 = await ctx.Orders.Include(o => o.Delivery)
    .FirstAsync(o => o.Name.Contains("充电器"));
Console.WriteLine($"名称：{order1.Name},单号：{order1.Delivery.Number}");

Console.WriteLine("一对一执行完毕");
#elif 多对多

#region 添加数据
//Student s1 = new Student { Name = "tom" };
//Student s2 = new Student { Name = "lily" };
//Student s3 = new Student { Name = "lucy" };
//Student s4 = new Student { Name = "tim" };
//Student s5 = new Student { Name = "lina" };

//Teacher t1 = new Teacher { Name = "杨中科" };
//Teacher t2 = new Teacher { Name = "张三" };
//Teacher t3 = new Teacher { Name = "李四" };

//t1.Students.Add(s1);
//t1.Students.Add(s2);
//t1.Students.Add(s3);

//t2.Students.Add(s1);
//t2.Students.Add(s3);
//t2.Students.Add(s5);

//t3.Students.Add(s2);
//t3.Students.Add(s4);

//ctx.AddRange(t1, t2, t3);
////ctx.AddRange(s1, s2, s3, s4, s5);
//await ctx.SaveChangesAsync();
#endregion

#region 查询数据

//foreach (var t in ctx.Teachers.Include(t => t.Students))
//{
//    Console.WriteLine($"老师{t.Name}");
//    foreach (var s in t.Students)
//    {
//        Console.WriteLine($"---{s.Name}");
//    }
//}
#endregion
#elif 基于关系的复杂查询
//var articles = ctx.Articles
//    .Where(a =>
//        a.Comments.Any(c => c.Message.Contains("微软")));
//foreach (var article in articles)
//{
//    Console.WriteLine($"{article.Id},{article.Title}");
//}

var articles = ctx.Comments
    .Where(c => c.Message.Contains("微软"))
    .Select(c => c.Article)
    .Distinct();
foreach (var article in articles)
{
    Console.WriteLine($"{article.Id},{article.Title}");
}
#endif