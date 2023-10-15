using IdentityService.Domain;
using IdentityService.Domain.Entities;
using IdentityService.Infrastructure;
using IdentityService.WebAPI.Events;
using IdentityService.WebAPI.RequestModels;
using IdentityService.WebAPI.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zack.EventBus;

namespace IdentityService.WebAPI.Controllers;
[Route("[controller]/[action]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UserAdminController : ControllerBase
{
    private readonly IdUserManager _userManager;
    private readonly IIdRepository _repository;
    private readonly IEventBus _eventBus;

    public UserAdminController(IdUserManager userManager, IIdRepository repository, IEventBus eventBus)
    {
        _userManager = userManager;
        _repository = repository;
        _eventBus = eventBus;
    }

    [HttpGet]
    public Task<UserAdminResponse[]> FindAllUsers()
    {
        return _userManager.Users.Select(u => UserAdminResponse.Create(u)).ToArrayAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<UserAdminResponse>> FindById(Guid id)
    {
        var user = await _repository.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound("用户未找到");
        }

        return UserAdminResponse.Create(user);
    }

    [HttpPost]
    public async Task<ActionResult> AddAdminUser(AddAdminUserRequest req)
    {
        var (r, u, pwd) = await _repository.AddAdminUserAsync(req.UserName, req.PhoneNum);
        if (!r.Succeeded)
        {
            return BadRequest(r.Errors.SumErrors());
        }

        /*
         * 生成的密码可以短信发给对方，或者邮件
         * 体现了集成对于代码“高内聚，低耦合”的追求
         */

        User admin = u!;
        var data = new UserCreatedEventData(admin.Id, admin.UserName!, pwd!, admin.PhoneNumber!);
        _eventBus.Publish("IdentityService.UserAdmin.Created", data);
        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeleteAdminUser(Guid id)
    {
        await _repository.RemoveUserAsync(id);
        return Ok();
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> UpdateAdminUser(Guid id, EditAdminUserRequest req)
    {
        var user = await _repository.FindByIdAsync(id);
        if (user == null) { return NotFound("用户未找到"); }

        var result = await _repository.UpdatePhoneNumberAsync(user.Id, req.PhoneNum);
        if (result.Succeeded)
            return Ok();

        return BadRequest(result.Errors.SumErrors());
    }

    [HttpPost]
    [Route("{id}")]
    public async Task<ActionResult> ResetAdminUserPassword(Guid id)
    {
        var (r, u, pwd) = await _repository.ResetPasswordAsync(id);
        if (!r.Succeeded)
        {
            return BadRequest(r.Errors.SumErrors());
        }

        User user = u!;
        //生成的密码短信发给对方
        var eventData = new ResetPasswordEventData(user.Id, user.UserName!, pwd!, user.PhoneNumber!);
        _eventBus.Publish("IdentityService.UserAdmin.PasswordReset", eventData);
        return Ok();
    }
}
