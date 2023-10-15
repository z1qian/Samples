using EFCoreLib;
using ExpressionTreeToString;
using System.Linq.Expressions;

//Expression和Func的区别
using TestDBContext ctx = new TestDBContext();

////Expression
//ctx.Books.Where(b => b.Price > 5).ToList();

////Func
//Func<Book, bool> e = b => b.Price > 5;
//ctx.Books.Where(e).ToList();

//Func<Book, bool> f1 = b => b.Price > 5 || b.AuthorName!.Contains("杨中科");
Expression<Func<Book, bool>> e =
    b2 =>
    b2.Price > 5
    ||
    b2.AuthorName!.Contains("杨中科");
//Console.WriteLine(f1);
//Console.WriteLine(e);

//生成源代码时使用的编程语言（以C#来输出每个节点的类型及节点的属性值）
Console.WriteLine(e.ToString("Object notation", "C#"));
