using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;

namespace EFCore中实现值对象;

internal class 简化值对象的比较帮助类
{
    public static Expression<Func<TItem, bool>> MakeEqual<TItem, TProp>(Expression<Func<TItem, TProp>> propAccessor, TProp? other)
        where TItem : class
        where TProp : class
    {
        var e1 = propAccessor.Parameters.Single();

        BinaryExpression? conditionalExpr = null;
        foreach (var prop in typeof(TProp).GetProperties())
        {
            object? otherValue = null;

            if (other != null)
                otherValue = prop.GetValue(other);

            var leftExpr = MakeMemberAccess(propAccessor.Body, prop);

            Type propType = prop.PropertyType;
            Expression rightExpr = Convert(Constant(otherValue), propType);

            BinaryExpression equalExpr;
            if (propType.IsPrimitive)
            {
                equalExpr = Equal(leftExpr, rightExpr);
            }
            else
            {
                equalExpr = MakeBinary(ExpressionType.Equal,
                   leftExpr, rightExpr, false,
                   prop.PropertyType.GetMethod("op_Equality"));
            }
            if (conditionalExpr == null)
                conditionalExpr = equalExpr;
            else
                conditionalExpr = AndAlso(conditionalExpr, equalExpr);
        }
        return Lambda<Func<TItem, bool>>(conditionalExpr, e1);
    }
}
