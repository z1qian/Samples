using Microsoft.Extensions.Configuration;

ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
/*
 * 1.不能泄露到源码中的配置放到user-secrets即可，不用都放
 * 2.一般把user-secrets优先级放到普通json文件之后
 * 3.如果开发人员电脑重装系统等原因造成本地的配置文件删除了，就需要重新配置
 * 4.并不是生产中的加密，只适用于开发
 * 5.用户机密文件，不包含在项目文件中
 */
configurationBuilder.AddUserSecrets<Program>();
var config = configurationBuilder.Build();

Console.WriteLine(config["Name"] is null ? "Null" : config["Name"]);