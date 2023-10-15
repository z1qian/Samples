using EFCoreLib;
using ExpressionTreeToString;
using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;

using TestDBContext ctx = new TestDBContext();

//Expression<Func<Book, bool>> expr1 = b => b.Price == 5;
//Expression<Func<Book, bool>> expr2 = b => b.Title == "零基础趣学C语言";
//Console.WriteLine(expr1.ToString("Factory methods", "C#"));
//Console.WriteLine(expr2.ToString("Factory methods", "C#"));

QueryBooks("Price", 18.0);
QueryBooks("AuthorName", "杨中科");
QueryBooks("Title", "零基础趣学C语言");
IEnumerable<Book> QueryBooks<T>(string propName, T value)
{
    var propType = typeof(Book).GetProperty(propName).PropertyType;

    var b = Parameter(
    typeof(Book),
    "b"
);

    Expression<Func<Book, bool>> expr;

    //如果是int、double等基本数据类型
    if (propType.IsPrimitive)
    {
        expr = Lambda<Func<Book, bool>>(
    Equal(
        MakeMemberAccess(b,
            typeof(Book).GetProperty(propName)
        ),
        Constant(value)
    ),
    b
);
    }
    else
    {
        expr = Lambda<Func<Book, bool>>(
            MakeBinary(ExpressionType.Equal,
                MakeMemberAccess(b,
                    typeof(Book).GetProperty(propName)
                ),
                Constant(value), false,
                typeof(string).GetMethod("op_Equality")
            ),
            b
        );
    }

    return ctx.Books.Where(expr).ToArray();
}