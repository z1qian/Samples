﻿using IdentityService.Domain;
using IdentityService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Zack.Commons;

namespace IdentityService.Infrastructure;

class ModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<IEmailSender, MockEmailSender>();
        services.AddScoped<ISmsSender, MockSmsSender>();
        services.AddScoped<IIdRepository, IdRepository>();
    }
}