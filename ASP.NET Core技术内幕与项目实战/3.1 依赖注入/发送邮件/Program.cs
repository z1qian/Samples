using ConfigReaderService;
using ConfigService;
using MailService;
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new ServiceCollection();

services.AddScoped<IMailProvider, QQMailProvider>();
services.AddLog();

//注册配置服务
services.AddScoped<IConfigProvider, EnviromentConfigProvider>();
services.AddScoped<IConfigProvider, DBConfigProvider>();
services.AddIniConfig("main.ini");

services.AddScoped<IConfigReader, LayeredConfigReader>();

using ServiceProvider sp = services.BuildServiceProvider();
//第一个根对象只能用ServiceLocator
IMailProvider mailProvider = sp.GetRequiredService<IMailProvider>();
mailProvider.Send("我是Title", "148392201@qq.com", "我是Body");