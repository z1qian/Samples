using LogService;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConsoleLogExtensions
    {
        public static void AddLog(this IServiceCollection services)
        {
            services.AddScoped<ILogProvider, ConsoleLogProvider>();
        }
    }
}