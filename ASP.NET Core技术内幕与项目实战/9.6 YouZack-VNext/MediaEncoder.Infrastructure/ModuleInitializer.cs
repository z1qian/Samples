using MediaEncoder.Domain;
using Microsoft.Extensions.DependencyInjection;
using Zack.Commons;

namespace MediaEncoder.Infrastructure;

class ModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<IMediaEncoderRepository, MediaEncoderRepository>();
        services.AddScoped<IMediaEncoder, ToM4AEncoder>();
    }
}