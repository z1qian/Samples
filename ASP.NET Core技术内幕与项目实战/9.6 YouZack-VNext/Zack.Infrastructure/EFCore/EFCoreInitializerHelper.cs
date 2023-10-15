﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore;

public static class EFCoreInitializerHelper
{
    // <summary>
    /// 自动为所有的DbContext注册连接配置
    /// </summary>
    public static IServiceCollection AddAllDbContexts(this IServiceCollection services, Action<DbContextOptionsBuilder> builder,
        IEnumerable<Assembly> assemblies)
    {
        //AddDbContextPool不支持DbContext注入其他对象，而且使用不当有内存暴涨的问题，因此不用AddDbContextPool
        Type[] methodParamTypes = new Type[] { typeof(IServiceCollection), typeof(Action<DbContextOptionsBuilder>), typeof(ServiceLifetime), typeof(ServiceLifetime) };

        MethodInfo methodAddDbContext = typeof(EntityFrameworkServiceCollectionExtensions)
            .GetMethod(nameof(EntityFrameworkServiceCollectionExtensions.AddDbContext), 1, methodParamTypes)!;

        foreach (var asm in assemblies)
        {
            /*
             * GetTypes() include public/protected ones
             * GetExportedTypes only include public ones
             * so that XXDbContext in Agrregation can be internal to keep insulated
             */
            Type[] typesInAsm = asm.GetTypes();

            //Register DbContext
            foreach (var dbCtxType in typesInAsm
                .Where(t => !t.IsAbstract && typeof(DbContext).IsAssignableFrom(t)))
            {
                MethodInfo methodGenericAddDbContext = methodAddDbContext.MakeGenericMethod(dbCtxType);
                methodGenericAddDbContext.Invoke(null, new object[] { services, builder, ServiceLifetime.Scoped, ServiceLifetime.Scoped });
            }
        }

        return services;
    }

}
