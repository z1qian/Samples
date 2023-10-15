using Listening.Admin.WebAPI.Categories.Request;
using Listening.Domain;
using Listening.Domain.Entities;
using Listening.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Zack.ASPNETCore;

namespace Listening.Admin.WebAPI.Categories;
[Route("[controller]/[action]")]
[ApiController]
[Authorize(Roles = "Admin")]
[UnitOfWork(typeof(ListeningDbContext))]
//供后台用的增删改查接口不用缓存
public class CategoryController : ControllerBase
{
    private readonly IListeningRepository _repository;
    private readonly ListeningDbContext _dbContext;
    private readonly ListeningDomainService _domainService;

    public CategoryController(IListeningRepository repository, ListeningDbContext dbContext, ListeningDomainService domainService)
    {
        _repository = repository;
        _dbContext = dbContext;
        _domainService = domainService;
    }

    [HttpGet]
    public Task<Category[]> FindAll()
    {
        return _repository.GetCategoriesAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Category>> FindById([RequiredGuid] Guid id)
    {
        //返回ValueTask的需要await的一下
        var cat = await _repository.GetCategoryByIdAsync(id);
        if (cat == null)
        {
            return NotFound($"没有Id={id}的Category");
        }

        return cat;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Add(CategoryAddRequest req)
    {
        var category = await _domainService.AddCategoryAsync(req.Name, req.CoverUrl);
        _dbContext.Add(category);
        return category.Id;
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, CategoryUpdateRequest request)
    {
        var cat = await _repository.GetCategoryByIdAsync(id);
        if (cat == null)
        {
            return NotFound("id不存在");
        }
        cat.ChangeName(request.Name);
        cat.ChangeCoverUrl(request.CoverUrl);
        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeleteById([RequiredGuid] Guid id)
    {
        var cat = await _repository.GetCategoryByIdAsync(id);
        if (cat == null)
        {
            //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
            return NotFound($"没有Id={id}的Category");
        }
        cat.SoftDelete();//软删除
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> Sort(CategoriesSortRequest req)
    {
        await _domainService.SortCategoriesAsync(req.SortedCategoryIds);
        return Ok();
    }
}
