using Microsoft.Extensions.DependencyInjection;
using Zack.Commons;

namespace FileService.Domain;

class ModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<FSDomainService>();
    }
}