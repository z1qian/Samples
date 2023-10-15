using Microsoft.Extensions.DependencyInjection;
using Zack.Commons;

namespace IdentityService.Domain;

class ModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<IdDomainService>();
    }
}
