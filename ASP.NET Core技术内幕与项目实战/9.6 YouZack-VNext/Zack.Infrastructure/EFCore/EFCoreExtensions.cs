using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using Zack.DomainCommons.Models;

namespace Microsoft.EntityFrameworkCore;

public static class EFCoreExtensions
{
    /// <summary>
    /// set global 'IsDeleted=false' queryfilter for every entity
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void EnableSoftDeletionGlobalFilter(this ModelBuilder modelBuilder)
    {
        var entityTypesHasSoftDeletion = modelBuilder.Model.GetEntityTypes()
            .Where(e => e.ClrType.IsAssignableTo(typeof(ISoftDelete)));

        foreach (var entityType in entityTypesHasSoftDeletion)
        {
            IMutableProperty isDeletedProperty = entityType.FindProperty(nameof(ISoftDelete.IsDeleted))!;
            var parameter = Expression.Parameter(entityType.ClrType, "p");
            var filter = Expression.Lambda(Expression.Not(Expression.Property(parameter, isDeletedProperty.PropertyInfo!)), parameter);
            entityType.SetQueryFilter(filter);
        }
    }

    public static IQueryable<T> Query<T>(this DbContext ctx, bool asNoTracking = true)
        where T : class, IEntity
    {
        if (asNoTracking)
        {
            return ctx.Set<T>().AsNoTracking();
        }

        return ctx.Set<T>();
    }
}
