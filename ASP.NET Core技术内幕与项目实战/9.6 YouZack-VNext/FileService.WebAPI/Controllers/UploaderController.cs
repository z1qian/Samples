using FileService.Domain;
using FileService.Infratructure;
using FileService.WebAPI.RequestModels;
using FileService.WebAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zack.ASPNETCore;

namespace FileService.WebAPI.Controllers;
[Route("[controller]/[action]")]
[ApiController]
[Authorize(Roles = "Admin")]
[UnitOfWork(typeof(FSDBContext))]
public class UploaderController : ControllerBase
{
    private readonly FSDBContext _dbContext;
    private readonly FSDomainService _domainService;
    private readonly IFSRepository _repository;

    public UploaderController(FSDBContext dbContext, FSDomainService domainService, IFSRepository repository)
    {
        _dbContext = dbContext;
        _domainService = domainService;
        _repository = repository;
    }


    /// <summary>
    /// 检查是否有和指定的大小和SHA256完全一样的文件
    /// </summary>
    [HttpGet]
    public async Task<FileExistsResponse> FileExists(long fileSize, string sha256Hash)
    {
        var item = await _repository.FindFileAsync(fileSize, sha256Hash);
        if (item == null)
        {
            return new FileExistsResponse(false, null);
        }

        return new FileExistsResponse(true, item.RemoteUrl);
    }

    [HttpPost]
    [RequestSizeLimit(60_000_000)]
    public async Task<ActionResult<Uri>> Upload([FromForm] UploadRequest request, CancellationToken cancellationToken = default)
    {
        var file = request.File;
        string fileName = file.FileName;

        using Stream stream = file.OpenReadStream();
        var upItem = await _domainService.UploadAsync(stream, fileName, cancellationToken);
        _dbContext.Add(upItem);

        return upItem.RemoteUrl;
    }
}
