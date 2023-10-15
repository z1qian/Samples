using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BooksEFCore;

public static class 批量注册上下文帮助类
{
    public static IServiceCollection AddAllDbContexts(this IServiceCollection services,
         Action<DbContextOptionsBuilder> builder, IEnumerable<Assembly> assemblies)
    {
        Type[] types = new Type[]
        {
            typeof(IServiceCollection),
            typeof(Action<DbContextOptionsBuilder>),
            typeof(ServiceLifetime),
            typeof(ServiceLifetime)
        };

        var methodAddDbContext = typeof(EntityFrameworkServiceCollectionExtensions)
           .GetMethod("AddDbContext", 1, types);

        foreach (var asmToLoad in assemblies)
        {
            foreach (var dbCtxType in asmToLoad.GetTypes()
               .Where(t => !t.IsAbstract && typeof(DbContext).IsAssignableFrom(t)))
            {
                var methodGenericAddDbContext = methodAddDbContext.MakeGenericMethod(dbCtxType);
                methodGenericAddDbContext.Invoke(null, new object[] { services, builder, ServiceLifetime.Scoped, ServiceLifetime.Scoped });
            }
        }
        return services;
    }
}
