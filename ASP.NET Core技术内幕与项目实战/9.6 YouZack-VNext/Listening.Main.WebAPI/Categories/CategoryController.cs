using Listening.Domain;
using Listening.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Zack.ASPNETCore;

namespace Listening.Main.WebAPI.Categories;
[Route("[controller]/[action]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IListeningRepository _repository;
    private readonly IMemoryCacheHelper _cacheHelper;
    public CategoryController(IListeningRepository repository, IMemoryCacheHelper cacheHelper)
    {
        _repository = repository;
        _cacheHelper = cacheHelper;
    }

    [HttpGet]
    public async Task<ActionResult<CategoryVM[]?>> FindAll()
    {
        Task<Category[]> FindData()
        {
            return _repository.GetCategoriesAsync();
        }

        string cacheKey = this.CalcCacheKeyFromAction();
        CategoryVM[]? categories = await _cacheHelper.GetOrCreateAsync(cacheKey,
              async (e) => CategoryVM.Create(await FindData()));
        return categories;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<CategoryVM?>> FindById([RequiredGuid] Guid id)
    {
        string cacheKey = this.CalcCacheKeyFromAction();
        var cat = await _cacheHelper.GetOrCreateAsync(cacheKey,
            async (e) => CategoryVM.Create(await _repository.GetCategoryByIdAsync(id)));
        //返回ValueTask的需要await的一下
        if (cat == null)
        {
            return NotFound($"没有Id={id}的Category");
        }

        return cat;
    }
}
