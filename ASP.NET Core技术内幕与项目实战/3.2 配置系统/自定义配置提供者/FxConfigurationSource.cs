using Microsoft.Extensions.Configuration;

namespace 自定义配置提供者;

//主要是提供参数使用
internal class FxConfigurationSource : FileConfigurationSource
{
    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        //处理Path等默认值问题
        EnsureDefaults(builder);
        return new FxConfigurationProvider(this);
    }
}
