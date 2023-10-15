using FileService.Domain;
using FileService.Infratructure.StorageServices;
using Microsoft.Extensions.DependencyInjection;
using Zack.Commons;

namespace FileService.Infratructure;

class ModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        //services.AddHttpClient();
        services.AddHttpContextAccessor();
        services.AddScoped<IStorageClient, MockCloudStorageClient>();
        services.AddScoped<IStorageClient, SMBStorageClient>();
        services.AddScoped<IFSRepository, FSRepository>();
    }
}
