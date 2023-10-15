using EFCoreLib;

//动态构建表达式树的代码仍然非常复杂，这些代码易读性差、可维护性差。因此在进行项目开发的时候，如果我们能用分步构建IQueryable等方式的话，
//就要尽量避免动态构建表达式树。
Book[] QueryBooks(string title, double? lowerPrice, double? upperPrice, int orderByType)
{
    using TestDBContext ctx = new TestDBContext();

    IQueryable<Book> source = ctx.Books;

    if (!string.IsNullOrEmpty(title))
        source = source.Where(b => b.Title.Contains(title));

    if (lowerPrice != null)
        source = source.Where(b => b.Price >= lowerPrice);

    if (upperPrice != null)
        source = source.Where(b => b.Price <= upperPrice);

    if (orderByType == 1)
        source = source.OrderByDescending(b => b.Price);
    else if (orderByType == 2)
        source = source.OrderBy(b => b.Price);

    return source.ToArray();
}