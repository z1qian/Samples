using EFCoreLib;

/*
 *  EF Core会尝试按照命名规则去直接读写属性对应的成员变量，
 *  只有无法根据命名规则找到对应成员变量的时候，EF Core才会通过属性的get、set代码块来读写属性值
 */
using TestDBContext ctx = new TestDBContext();

//Book b1 = new Book()
//{
//    AuthorName = "zzx",
//    Price = 23.22,
//    PubTime = DateTime.Now,
//    Remark = string.Empty,
//    Title = "测试图书19",
//};
//Console.WriteLine("book初始化完毕");
//ctx.Books.Add(b1);
//ctx.SaveChanges();
//Console.WriteLine("SaveChanges完毕");

Console.WriteLine("准备读取数据");
Book b2 = ctx.Books.First();
Console.WriteLine("读取数据完毕");
