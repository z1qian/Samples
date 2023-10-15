using Lib;

using TestDBContext ctx = new TestDBContext();

OutputPage(1, 5);
Console.WriteLine("******");
OutputPage(2, 5);

void OutputPage(int pageIndex, int pageSize)
{
    IQueryable<Book> books = ctx.Books
        .Where(b => !b.Title!.Contains("张三"));

    //总条数
    long count = books.LongCount();
    //页数
    long pageCount = (long)Math.Ceiling(count * 1.0 / pageSize);
    Console.WriteLine("页数：" + pageCount);

    var pagedBooks = books
        .Skip((pageIndex - 1) * pageSize)
        .Take(pageSize);
    foreach (var b in pagedBooks)
    {
        Console.WriteLine(b.Id + "," + b.Title);
    }
}