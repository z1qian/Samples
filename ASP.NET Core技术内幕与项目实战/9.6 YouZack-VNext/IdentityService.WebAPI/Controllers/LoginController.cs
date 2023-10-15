using IdentityService.Domain;
using IdentityService.Domain.Entities;
using IdentityService.WebAPI.RequestModels;
using IdentityService.WebAPI.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

namespace IdentityService.WebAPI.Controllers;
[Route("[controller]/[action]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IIdRepository _repository;
    private readonly IdDomainService _idService;

    public LoginController(IIdRepository repository, IdDomainService idService)
    {
        _repository = repository;
        _idService = idService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateWorld()
    {
        if (await _repository.FindByNameAsync("admin") != null)
        {
            return StatusCode((int)HttpStatusCode.Conflict, "已经初始化过了");
        }

        User user = new ("admin");
        var r = await _repository.CreateAsync(user, "123456");
        Debug.Assert(r.Succeeded);

        string token = await _repository.GenerateChangePhoneNumberTokenAsync(user, "13111111111");
        r = await _repository.ChangePhoneNumAsync(user.Id, "13111111111", token);
        Debug.Assert(r.Succeeded);

        r = await _repository.AddToRoleAsync(user, "User");
        Debug.Assert(r.Succeeded);
        r = await _repository.AddToRoleAsync(user, "Admin");
        Debug.Assert(r.Succeeded);
        return Ok();
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<UserResponse>> GetUserInfo()
    {
        string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid userIdGuid = string.IsNullOrWhiteSpace(userIdStr) ? Guid.Empty : Guid.Parse(userIdStr);

        var user = await _repository.FindByIdAsync(userIdGuid);
        if (user == null)
        {
            return NotFound();
        }

        /*
         * 出于安全考虑，不要把机密信息传递到客户端
         * 除非确认没问题，否则尽量不要把实体类对象直接返回给前端
         */
        return new UserResponse(user.Id, user.PhoneNumber, user.CreationTime);
    }

    [HttpPost]
    public async Task<ActionResult<string?>> LoginByPhoneAndPwd(LoginByPhoneAndPwdRequest req)
    {
        var (result, token) = await _idService.LoginByPhoneAndPwdAsync(req.PhoneNum, req.Password);
        if (result.Succeeded)
        {
            return token;
        }
        else if (result.IsLockedOut)
        {
            return StatusCode(StatusCodes.Status423Locked, "此账号已被锁定");
        }
        else
        {
            return BadRequest("登录失败");
        }
    }

    [HttpPost]
    public async Task<ActionResult<string?>> LoginByUserNameAndPwd(LoginByUserNameAndPwdRequest req)
    {
        var (result, token) = await _idService.LoginByUserNameAndPwdAsync(req.UserName, req.Password);
        if (result.Succeeded)
        {
            return token;
        }
        else if (result.IsLockedOut)
        {
            return StatusCode(StatusCodes.Status423Locked, "此账号已被锁定");
        }
        else
        {
            return BadRequest(result.ToString());
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> ChangeMyPassword(ChangeMyPasswordRequest req)
    {
        string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid userIdGuid = string.IsNullOrWhiteSpace(userIdStr) ? Guid.Empty : Guid.Parse(userIdStr);
        var resetPwdResult = await _repository.ChangePasswordAsync(userIdGuid, req.Password);
        if (resetPwdResult.Succeeded)
        {
            return Ok();
        }
        else
        {
            return BadRequest(resetPwdResult.Errors.SumErrors());
        }
    }
}
