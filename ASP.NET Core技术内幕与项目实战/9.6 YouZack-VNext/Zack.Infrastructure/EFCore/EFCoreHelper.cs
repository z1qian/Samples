using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace Zack.Infrastructure.EFCore;

public static class EFCoreHelper
{
    /// <summary>
    /// 得到表名
    /// </summary>
    public static string? GetTableName<TEntity>(this DbContext dbCtx)
    {
        var entityType = dbCtx.Model.FindEntityType(typeof(TEntity))
            ?? throw new Exception("TEntity is not found in DbContext");

        return entityType.GetTableName();
    }

    /// <summary>
    /// 得到实体中属性对应的列名
    /// 用法：string fName = dbCtx.GetColumnName<Person>(p=>p.Name);
    /// </summary>

    public static string? GetColumnName<TEntity>(this DbContext dbCtx, Expression<Func<TEntity, object>> propertyLambda)
    {
        var entityType = dbCtx.Model.FindEntityType(typeof(TEntity))
            ?? throw new Exception("TEntity is nof found in DbContext");

        var member = propertyLambda.Body as MemberExpression;
        if (member == null)
        {
            if (propertyLambda.Body is UnaryExpression unary)
            {
                member = unary.Operand as MemberExpression;
            }
        }
        if (member == null)
        {
            throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.",
                propertyLambda.ToString()));
        }

        if (member.Member is not PropertyInfo propInfo)
        {
            throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.",
             propertyLambda.ToString()));
        }

        Type type = typeof(TEntity);
        if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType!))
        {
            throw new ArgumentException(string.Format("Expression '{0}' refers to a property that is not from type {1}.",
                propertyLambda.ToString(), type));
        }

        string propertyName = propInfo.Name;
        var objId = StoreObjectIdentifier.Create(entityType, StoreObjectType.Table);
        if (objId == null)
            return null;

        return entityType.FindProperty(propertyName)?.GetColumnName(objId.Value);
    }
}
