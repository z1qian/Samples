using FileService.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FileService.Infratructure.StorageServices;

public class MockCloudStorageClient : IStorageClient
{
    private readonly IWebHostEnvironment _hostEnv;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MockCloudStorageClient(IWebHostEnvironment hostEnv, IHttpContextAccessor httpContextAccessor)
    {
        _hostEnv = hostEnv;
        _httpContextAccessor = httpContextAccessor;
    }

    public StorageType StorageType => StorageType.Public;

    public async Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken = default)
    {
        if (key.StartsWith("/"))
        {
            throw new ArgumentException("key should not start with /", nameof(key));
        }

        string workingDir = Path.Combine(_hostEnv.ContentRootPath, "wwwroot");
        string filePath = Path.Combine(workingDir, key);
        string fileDir = Path.GetDirectoryName(filePath)!;
        //不存在则创建
        Directory.CreateDirectory(fileDir);
        if (!File.Exists(filePath))
        {
            using Stream outStream = File.OpenWrite(filePath);
            await content.CopyToAsync(outStream, cancellationToken);
        }
        var req = _httpContextAccessor.HttpContext?.Request;
        string url = filePath;
        if (req != null)
        {
            url = $"{req.Scheme}://{req.Host}/FileService/{key}";
        }
        return new Uri(url);
    }
}
