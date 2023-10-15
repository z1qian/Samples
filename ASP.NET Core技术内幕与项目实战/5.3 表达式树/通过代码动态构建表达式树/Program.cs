using EFCoreLib;
using ExpressionTreeToString;
using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;

using TestDBContext ctx = new TestDBContext();

////创建表达式树的工厂方法
//ParameterExpression paramB = Expression.Parameter(typeof(Book), "b");
//MemberExpression exprLeft = Expression.MakeMemberAccess(paramB,
//   typeof(Book).GetProperty("Price"));
//ConstantExpression exprRight = Expression.Constant(5.0, typeof(double));
//BinaryExpression exprBody = Expression.MakeBinary(ExpressionType.GreaterThan,
//   exprLeft, exprRight);
//Expression<Func<Book, bool>> expr1 = Expression.Lambda<Func<Book, bool>>
//(exprBody, paramB);

//ctx.Books.Where(expr1).ToList();

//Console.WriteLine(expr1.ToString("Object notation", "C#"));

////生成通过调用工厂方法构建表达式树的代码
//Expression<Func<Book, bool>> e = b => b.AuthorName.Contains("杨中科") || b.Price > 30;
//Console.WriteLine(e.ToString("Factory methods", "C#"));


var b = Parameter(
    typeof(Book),
    "b"
);

var lam = Lambda<Func<Book, bool>>(
    OrElse(
        Call(
            MakeMemberAccess(b,
                typeof(Book).GetProperty("AuthorName")
            ),
            typeof(string).GetMethod("Contains", new[] { typeof(string) }),
            Constant("杨中科")
        ),
        GreaterThan(
            MakeMemberAccess(b,
                typeof(Book).GetProperty("Price")
            ),
            Constant(30.0)
        )
    ),
    b
);

Console.WriteLine(lam);

