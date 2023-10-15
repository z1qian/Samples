using Listening.Domain;
using Listening.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Zack.ASPNETCore;

namespace Listening.Main.WebAPI.Episodes;
[Route("[controller]/[action]")]
[ApiController]
public class EpisodeController : ControllerBase
{
    private readonly IListeningRepository _repository;
    private readonly IMemoryCacheHelper _cacheHelper;

    public EpisodeController(IMemoryCacheHelper cacheHelper, IListeningRepository repository)
    {
        _cacheHelper = cacheHelper;
        _repository = repository;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<EpisodeVM>> FindById([RequiredGuid] Guid id)
    {
        var episode = await _cacheHelper.GetOrCreateAsync(this.CalcCacheKeyFromAction(),
            async (e) => EpisodeVM.Create(await _repository.GetEpisodeByIdAsync(id), true));
        if (episode == null)
        {
            return NotFound($"没有Id={id}的Episode");
        }
        return episode;
    }

    [HttpGet]
    [Route("{albumId}")]
    public async Task<ActionResult<EpisodeVM[]?>> FindByAlbumId([RequiredGuid] Guid albumId)
    {
        Task<Episode[]> FindData()
        {
            return _repository.GetEpisodesByAlbumIdAsync(albumId);
        }

        //加载Episode列表的，默认不加载Subtitle，这样降低流量大小
        var episodes = await _cacheHelper.GetOrCreateAsync(this.CalcCacheKeyFromAction(),
            async (e) => EpisodeVM.Create(await FindData(), false));
        return episodes;
    }
}
