using Microsoft.Extensions.DependencyInjection;
using Zack.Commons;
using 例子服务接口1;

ServiceCollection services = new ServiceCollection();

var assemblies = ReflectionHelper.GetAllReferencedAssemblies();//获取所有用户程序集

services.RunModuleInitializers(assemblies);                //运行所有程序集中的服务注册

using var sp = services.BuildServiceProvider();
var items = sp.GetServices<IMyService>();
foreach (var item in items)
{
    item.SayHello();
}