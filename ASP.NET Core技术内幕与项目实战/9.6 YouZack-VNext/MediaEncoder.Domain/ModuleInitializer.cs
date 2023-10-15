using Microsoft.Extensions.DependencyInjection;
using Zack.Commons;

namespace MediaEncoder.Domain;

class ModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<MediaEncoderFactory>();
    }
}