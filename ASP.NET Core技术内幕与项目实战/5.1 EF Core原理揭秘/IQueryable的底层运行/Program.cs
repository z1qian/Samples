/*
 * IQueryable是用类似DataReader的方式读取查询结果的。
 * 其实IQueryable内部的遍历就是在调用DataReader进行数据读取。
 * 因此，在遍历IQueryable的过程中，它需要占用一个数据库连接。
 */

using Lib;

//foreach (var b in QueryBooks())
//{
//    Console.WriteLine(b.Title);
//}

//////错误的返回查询结果
////IQueryable<Book> QueryBooks()
////{
////    using TestDBContext ctx = new TestDBContext();
////    return ctx.Books.Where(b => b.Id >= 1);
////}

//IEnumerable<Book> QueryBooks()
//{
//    using TestDBContext ctx = new TestDBContext();
//    return ctx.Books.Where(b => b.Id >= 1).ToArray();
//}

/*
 * IQueryable底层是使用DataReader从数据库服务器读取查询结果的，
 * 而很多数据库是不支持多个DataReader同时执行的。
 */
using TestDBContext ctx = new TestDBContext();
var books = ctx.Books.Where(b => b.Id >= 1).ToList();
foreach (var b in books)
{
    Console.WriteLine(b.Id + "," + b.Title);
    foreach (var a in ctx.Books)
    {
        Console.WriteLine(a.Id);
    }
}