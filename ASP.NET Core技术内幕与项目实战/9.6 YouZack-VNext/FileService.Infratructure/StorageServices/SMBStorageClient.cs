using FileService.Domain;
using Microsoft.Extensions.Options;

namespace FileService.Infratructure.StorageServices;

/// <summary>
/// 用局域网内共享文件夹或者本机磁盘当备份服务器的实现类
/// </summary>
public class SMBStorageClient : IStorageClient
{
    private readonly IOptionsSnapshot<SMBStorageOptions> _options;

    public SMBStorageClient(IOptionsSnapshot<SMBStorageOptions> options)
    {
        _options = options;
    }

    public StorageType StorageType => StorageType.Backup;


    public async Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken = default)
    {
        if (key.StartsWith("/"))
        {
            throw new ArgumentException("key should not start with /", nameof(key));
        }

        string? workingDir = _options.Value.WorkingDir;
        if (string.IsNullOrWhiteSpace(workingDir))
        {
            throw new Exception("未设置备份服务器根目录");
        }
        string filePath = Path.Combine(workingDir, key);
        string fileDir = Path.GetDirectoryName(filePath)!;
        //不存在则创建
        Directory.CreateDirectory(fileDir);
        if (!File.Exists(filePath))
        {
            using Stream outStream = File.OpenWrite(filePath);
            await content.CopyToAsync(outStream, cancellationToken);
        }

        return new Uri(filePath);
    }
}
