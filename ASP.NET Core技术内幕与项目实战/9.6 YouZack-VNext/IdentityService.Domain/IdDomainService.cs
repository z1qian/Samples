using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Zack.JWT;

namespace IdentityService.Domain;

public class IdDomainService
{
    private readonly IIdRepository _repository;
    private readonly ITokenService _tokenService;
    private readonly IOptions<JWTOptions> _optJWT;

    public IdDomainService(IIdRepository repository, ITokenService tokenService, IOptions<JWTOptions> optJWT)
    {
        _repository = repository;
        _tokenService = tokenService;
        _optJWT = optJWT;
    }

    private async Task<string> BuildTokenAsync(User user)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var roles = await _repository.GetRolesAsync(user);
        foreach (string role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return _tokenService.BuildToken(claims, _optJWT.Value);
    }

    private async Task<SignInResult> CheckPhoneNumAndPwdAsync(string phoneNum, string password)
    {
        var user = await _repository.FindByPhoneNumberAsync(phoneNum);
        if (user == null)
        {
            return SignInResult.Failed;
        }
        return await _repository.CheckForSignInAsync(user, password, true);
    }

    public async Task<(SignInResult Result, string? Token)> LoginByPhoneAndPwdAsync(string phoneNum, string password)
    {
        var checkResult = await CheckPhoneNumAndPwdAsync(phoneNum, password);
        if (checkResult.Succeeded)
        {
            User user = (await _repository.FindByPhoneNumberAsync(phoneNum))!;
            string token = await BuildTokenAsync(user);
            return (SignInResult.Success, token);
        }
        else
        {
            return (checkResult, null);
        }
    }

    private async Task<SignInResult> CheckUserNameAndPwdAsync(string userName, string password)
    {
        var user = await _repository.FindByNameAsync(userName);
        if (user == null)
        {
            return SignInResult.Failed;
        }
        //会对于多次重复失败进行账号禁用
        return await _repository.CheckForSignInAsync(user, password, true);
    }

    public async Task<(SignInResult Result, string? Token)> LoginByUserNameAndPwdAsync(string userName, string password)
    {
        var checkResult = await CheckUserNameAndPwdAsync(userName, password);
        if (checkResult.Succeeded)
        {
            User user = (await _repository.FindByNameAsync(userName))!;
            string token = await BuildTokenAsync(user);
            return (SignInResult.Success, token);
        }
        else
        {
            return (checkResult, null);
        }
    }
}
