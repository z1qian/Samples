using Lib;

using TestDBContext ctx = new TestDBContext();

//// 服务器端评估
//IQueryable<Book> books = ctx.Books;
//foreach (var b in books.Where(b => b.Price > 1.1))
//{
//    Console.WriteLine($"Id={b.Id},Title={b.Title}");
//}

////客户端评估
//IEnumerable<Book> books = ctx.Books;
//foreach (var b in books.Where(b => b.Price > 1.1))
//{
//    Console.WriteLine($"Id={b.Id},Title={b.Title}");
//}

////没有遍历输出结果
//IQueryable<Book> books = ctx.Books.Where(b => b.Price > 1.1);
//Console.WriteLine(books);

////遍历输出结果
////判断一个方法是否是立即执行方法的简单方式是：
////一个方法的返回值类型如果是IQueryable类型，这个方法一般就是非立即执行方法，
////否则这个方法就是立即执行方法。
//Console.WriteLine("1. Where之前");
//IQueryable<Book> books = ctx.Books.Where(b => b.Price > 1.1);
//Console.WriteLine("2. 遍历IQueryable之前");
//foreach (var b in books)
//{
//    Console.WriteLine(b.Title + ":" + b.PubTime);
//}
//Console.WriteLine("3. 遍历IQueryable之后");


////拼接复杂的查询条件
//QueryBooks("法", true, true, 110);

//void QueryBooks(string searchWords, bool searchAll, bool orderByPrice, double upperPrice)
//{
//    IQueryable<Book> books = ctx.Books.Where(b => b.Price <= upperPrice);
//    if (searchAll)      //匹配书名或作者名
//    {
//        books = books.Where(b => b.Title.Contains(searchWords) ||
//               b.AuthorName.Contains(searchWords));
//    }
//    else                //只匹配书名
//    {
//        books = books.Where(b => b.Title.Contains(searchWords));
//    }
//    if (orderByPrice)   //按照价格排序
//    {
//        books = books.OrderBy(b => b.Price);
//    }
//    foreach (Book b in books)
//    {
//        Console.WriteLine($"{b.Id},{b.Title},{b.Price},{b.AuthorName}");
//    }
//}

////IQueryable的复用
//IQueryable<Book> books = ctx.Books.Where(b => b.Price >= 8);

//Console.WriteLine(books.Count());
//Console.WriteLine(books.Max(b => b.Price));
//foreach (Book b in books.Where(b => b.PubTime.Year > 2000))
//{
//    Console.WriteLine(b.Title);
//}