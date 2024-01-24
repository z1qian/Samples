# SPA与WebAPI的集成

```c#
var builder = WebApplication.CreateBuilder(args);

// ...

builder.Services.AddSpaStaticFiles(options =>
{
    options.RootPath = "clientApp";
});

var app = builder.Build();

// ...

//app.UseStaticFiles(new StaticFileOptions()
//{
//    RequestPath = new Microsoft.AspNetCore.Http.PathString("/static"),
//    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "clientApp/static"))
//});

app.UseSpaStaticFiles();

app.UseSpa(spa =>
{
    spa.Options.DefaultPage = "/index.html";
    spa.Options.SourcePath = "clientApp";
});

//...
app.Run();
```

如果需要在 `WebAPI` 项目，一打开，就启动 `SPA` 界面，需要在 `launchSettings.json` 中，把 `launchUrl` 由 `swagger` 改成你想要的地址，比如：`admi`（注意：`launchSettings.json` 只适用于本地开发，发布后不会包含 `launchSettings.json` 文件）

## 代码解析

* `app.UseStaticFiles()` 如果没有提供参数，默认访问 `wwwroot` 文件夹下的静态文件，如果静态文件没有放到 `wwwroot` 下，则无法访问到静态文件

* ```c#
  app.UseStaticFiles(new StaticFileOptions()
  {
      RequestPath = new Microsoft.AspNetCore.Http.PathString("/static"),
      FileProvider = new 		Microsoft.Extensions.FileProviders.PhysicalFileProvider(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "clientApp/static"))
  });
  ```

  使用给定选项启用静态文件服务，当你访问 `/static` 地址下的静态文件时，它的物理文件路径为 `System.IO.Directory.GetCurrentDirectory() + "/clientApp/static")`

* `app.UseSpaStaticFiles()` 是对 `app.UseStaticFiles(new StaticFileOptions()...)` 的封装

* 可同时存在 `app.UseStaticFiles()` 与 `app.UseSpaStaticFiles()`，以同时达到对 `wwwroot` 和访问 `SPA` 页面的支持

* 在发布后，发布文件列表中不会包含 `SPA` 的文件夹（`clientApp`），需要在项目文件中（`.csproj`）添加如下代码

  ```c#
  <ItemGroup>
  	<!-- 包含 ClientApp 文件夹及其内容 -->
  	<Content Include="ClientApp\**" CopyToPublishDirectory="PreserveNewest" />
  </ItemGroup>
  ```

  