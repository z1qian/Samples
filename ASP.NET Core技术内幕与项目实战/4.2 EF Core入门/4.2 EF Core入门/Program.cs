//#define 插入数据
//#define 查询数据
//#define 修改和删除数据

using _4._2_EF_Core入门;

using TestDBContext ctx = new TestDBContext();

#if 插入数据
var b1 = new Book
{
    AuthorName = "杨中科",
    Title = "零基础趣学C语言",
    Price = 59.8,
    PubTime = new DateTime(2019, 3, 1),
};
var b2 = new Book
{
    AuthorName = "Robert Sedgewick",
    Title = "算法(第4版)",
    Price = 99,
    PubTime = new DateTime(2012, 10, 1)
};
var b3 = new Book
{
    AuthorName = "吴军",
    Title = "数学之美",
    Price = 69,
    PubTime = new DateTime(2020, 5, 1)
};
var b4 = new Book
{
    AuthorName = "杨中科",
    Title = "程序员的SQL金典",
    Price = 52,
    PubTime = new DateTime(2008, 9, 1)
};
var b5 = new Book
{
    AuthorName = "吴军",
    Title = "文明之光",
    Price = 246,
    PubTime = new DateTime(2017, 3, 1)
};
ctx.Books.Add(b1);
ctx.Books.Add(b2);
ctx.Books.Add(b3);
ctx.Books.Add(b4);
ctx.Books.Add(b5);
await ctx.SaveChangesAsync();
Console.WriteLine("插入数据成功");

#elif 查询数据
Console.WriteLine("***所有书***");
foreach (Book b in ctx.Books)
{
    Console.WriteLine($"Id={b.Id},Title={b.Title},Price={b.Price}");
}
Console.WriteLine("\n***所有价格高于80元的书***");
IEnumerable<Book> books2 = ctx.Books.Where(b => b.Price > 80);
foreach (Book b in books2)
{
    Console.WriteLine($"Id={b.Id},Title={b.Title},Price={b.Price}");
}

Console.WriteLine();

Book b1 = ctx.Books.Single(b => b.Title == "零基础趣学C语言");
Console.WriteLine($"Id={b1.Id},Title={b1.Title},Price={b1.Price}");

Book? b2 = ctx.Books.FirstOrDefault(b => b.Id == 9);
if (b2 == null)
    Console.WriteLine("没有Id=9的数据");
else
    Console.WriteLine($"Id={b2.Id},Title={b2.Title},Price={b2.Price}");

Console.WriteLine();

IEnumerable<Book> books = ctx.Books.OrderByDescending(b => b.Price);
foreach (Book b in books)
{
    Console.WriteLine($"Id={b.Id},Title={b.Title},Price={b.Price}");
}

Console.WriteLine();

var result = ctx.Books
    .GroupBy(b => b.AuthorName)
    .Select(g =>
    new
    {
        AutherName = g.Key,
        BooksCount = g.Count(),
        MaxPrice = g.Max(b => b.Price)
    });

foreach (var item in result)
{
    Console.WriteLine($"作者：{item.AutherName},图书数量：{item.BooksCount},最高价格:{item.MaxPrice}");
}
Console.WriteLine("查询数据完毕");
#elif 修改和删除数据
var b = ctx.Books.Single(b => b.Title == "数学之美");
b.AuthorName = "Jun Wu";

await ctx.SaveChangesAsync();
Console.WriteLine("修改【数学之美】成功");

var b2 = ctx.Books.Single(b => b.Title == "数学之美");
//ctx.Remove(b2);
ctx.Books.Remove(b2);
await ctx.SaveChangesAsync();
Console.WriteLine("删除【数学之美】成功");
#else
Console.WriteLine("请先定义操作");
#endif