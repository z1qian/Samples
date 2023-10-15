using Listening.Domain;
using Listening.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Zack.ASPNETCore;

namespace Listening.Main.WebAPI.Albums;
[Route("[controller]/[action]")]
[ApiController]
public class AlbumController : ControllerBase
{
    private readonly IListeningRepository _repository;
    private readonly IMemoryCacheHelper _cacheHelper;
    public AlbumController(IListeningRepository repository, IMemoryCacheHelper cacheHelper)
    {
        _repository = repository;
        _cacheHelper = cacheHelper;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<AlbumVM>> FindById([RequiredGuid] Guid id)
    {
        string cacheKey = this.CalcCacheKeyFromAction();
        var album = await _cacheHelper.GetOrCreateAsync(cacheKey,
           async (e) => AlbumVM.Create(await _repository.GetAlbumByIdAsync(id)));
        if (album == null)
        {
            return NotFound();
        }
        return album;
    }

    [HttpGet]
    [Route("{categoryId}")]
    public async Task<ActionResult<AlbumVM[]>> FindByCategoryId([RequiredGuid] Guid categoryId)
    {
        //写到单独的local函数的好处是避免回调中代码太复杂
        Task<Album[]> FindDataAsync()
        {
            return _repository.GetAlbumsByCategoryIdAsync(categoryId);
        }

        string cacheKey = this.CalcCacheKeyFromAction();
        var task = _cacheHelper.GetOrCreateAsync(cacheKey,
            async (e) => AlbumVM.Create(await FindDataAsync()));
        return (await task)!;
    }
}
