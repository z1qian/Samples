using 自定义配置提供者;

namespace Microsoft.Extensions.Configuration;

internal static class FxConfigurationExtensions
{
    public static IConfigurationBuilder AddFxConfig(this IConfigurationBuilder configurationBuilder, string path = null)
    {
        path ??= "web.config";

        configurationBuilder.Add(new FxConfigurationSource() { Path = path });

        return configurationBuilder;
    }
}
