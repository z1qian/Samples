using FileService.Domain.Entities;
using Zack.Commons;

namespace FileService.Domain;

public class FSDomainService
{
    private readonly IFSRepository _repository;
    private readonly IStorageClient _backupStorage;//备份服务器
    private readonly IStorageClient _remoteStorage;//文件存储服务器

    public FSDomainService(IFSRepository repository, IEnumerable<IStorageClient> storageClients)
    {
        _repository = repository;
        _backupStorage = storageClients.First(c => c.StorageType == StorageType.Backup);
        _remoteStorage = storageClients.First(c => c.StorageType == StorageType.Public);
    }

    public async Task<UploadedItem> UploadAsync(Stream stream, string fileName, CancellationToken cancellationToken)
    {
        string hash = HashHelper.ComputeSha256Hash(stream);
        long fileSize = stream.Length;
        DateTime today = DateTime.Today;

        /*
         * 用日期把文件分散在不同文件夹存储，同时由于加上了文件hash值作为目录，又用用户上传的文件夹做文件名，
         * 所以几乎不会发生不同文件冲突的可能
         * 用用户上传的文件名保存文件名，这样用户查看、下载文件的时候，文件名更灵活
         */

        string key = $"{today.Year}/{today.Month}/{today.Day}/{hash}/{fileName}";
        /*
         * 查询是否有和上传文件的大小和SHA256一样的文件，如果有的话，就认为是同一个文件
         * 虽然说前端可能已经调用FileExists接口检查过了，但是前端可能跳过了，或者有并发上传等问题，所以这里再检查一遍
         */
        var existedUploadItem = await _repository.FindFileAsync(fileSize, hash);
        if (existedUploadItem != null)
        {
            return existedUploadItem;
        }
        stream.Position = 0;
        //保存到本地备份
        Uri backupUrl = await _backupStorage.SaveAsync(key, stream, cancellationToken);
        stream.Position = 0;
        //保存到远程存储系统
        Uri remoteUrl = await _remoteStorage.SaveAsync(key, stream, cancellationToken);
        stream.Position = 0;

        /*
         * 领域服务并不会真正的执行数据库插入，只是把实体对象生成，然后由应用服务和基础设施配合来真正的插入数据库
         * DDD中尽量避免直接在领域服务中执行数据库的修改（包含删除、新增）操作
         */

        return new UploadedItem(Guid.NewGuid(), fileSize, fileName, hash, backupUrl, remoteUrl);
    }
}
