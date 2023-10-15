using ConfigService;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IniConfigExtensions
    {
        public static void AddIniConfig(this IServiceCollection services, string fileName)
        {
            services.AddScoped<IConfigProvider, IniConfigProvider>(s => new IniConfigProvider() { FilePath = fileName });
        }
    }
}
