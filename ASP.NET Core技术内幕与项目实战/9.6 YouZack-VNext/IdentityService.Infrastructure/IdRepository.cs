using IdentityService.Domain;
using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;

namespace IdentityService.Infrastructure;

public class IdRepository : IIdRepository
{
    private readonly IdUserManager _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<IdRepository> _logger;

    public IdRepository(IdUserManager userManager, RoleManager<Role> roleManager, ILogger<IdRepository> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    private static IdentityResult ErrorResult(string msg, string code = "")
    {
        IdentityError idError = new IdentityError { Description = msg, Code = code };
        return IdentityResult.Failed(idError);
    }

    private string GeneratePassword()
    {
        var options = _userManager.Options.Password;
        int length = options.RequiredLength;
        bool nonAlphanumeric = options.RequireNonAlphanumeric;
        bool digit = options.RequireDigit;
        bool lowercase = options.RequireLowercase;
        bool uppercase = options.RequireUppercase;

        StringBuilder password = new StringBuilder();
        Random random = new Random();

        while (password.Length < length)
        {
            char c = (char)random.Next(32, 126);
            password.Append(c);
            if (char.IsDigit(c))
                digit = false;
            else if (char.IsLower(c))
                lowercase = false;
            else if (char.IsUpper(c))
                uppercase = false;
            else if (!char.IsLetterOrDigit(c))
                nonAlphanumeric = false;
        }

        if (nonAlphanumeric)
            password.Append((char)random.Next(33, 48));
        if (digit)
            password.Append((char)random.Next(48, 58));
        if (lowercase)
            password.Append((char)random.Next(97, 123));
        if (uppercase)
            password.Append((char)random.Next(65, 91));

        return password.ToString();
    }

    public Task<IdentityResult> AccessFailedAsync(User user)
    {
        return _userManager.AccessFailedAsync(user);
    }

    public Task<User?> FindByNameAsync(string userName)
    {
        return _userManager.FindByNameAsync(userName);
    }

    public async Task<(IdentityResult, User?, string? password)> AddAdminUserAsync(string userName, string phoneNum)
    {
        if (await FindByNameAsync(userName) != null)
        {
            return (ErrorResult($"已经存在用户名{userName}"), null, null);
        }

        if (await FindByPhoneNumberAsync(phoneNum) != null)
        {
            return (ErrorResult($"已经存在手机号{phoneNum}"), null, null);
        }

        User user = new(userName)
        {
            PhoneNumber = phoneNum,
            PhoneNumberConfirmed = true,
        };

        string password = GeneratePassword();
        var result = await CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return (result, null, null);
        }

        result = await AddToRoleAsync(user, "Admin");
        if (!result.Succeeded)
        {
            return (result, null, null);
        }
        return (IdentityResult.Success, user, password);
    }

    public async Task<IdentityResult> AddToRoleAsync(User user, string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            Role role = new Role { Name = roleName };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded == false)
            {
                return result;
            }
        }
        return await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<IdentityResult> ChangePasswordAsync(Guid userId, string password)
    {
        var options = _userManager.Options.Password;
        if (password.Length < options.RequiredLength)
        {
            return ErrorResult("密码长度不能少于" + options.RequiredLength, "Password Length Invalid");
        }

        var user = await FindByIdAsync(userId);
        if (user == null)
        {
            return ErrorResult("用户不存在", "User Does Not Exist");
        }
        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return await _userManager.ResetPasswordAsync(user, token, password);
    }

    public async Task<IdentityResult> ChangePhoneNumAsync(Guid userId, string phoneNum, string token)
    {
        User user = await FindByIdAsync(userId) ??
              throw new ArgumentException($"{userId}的用户不存在", nameof(userId));

        var result = await _userManager.ChangePhoneNumberAsync(user, phoneNum, token);
        if (!result.Succeeded)
        {
            await _userManager.AccessFailedAsync(user);
            string errMsg = result.Errors.SumErrors();
            _logger.LogError($"{phoneNum}ChangePhoneNumAsync失败，错误信息：{errMsg}");
            return result;
        }
        await ConfirmPhoneNumberAsync(userId);
        return IdentityResult.Success;
    }

    public async Task<SignInResult> CheckForSignInAsync(User user, string password, bool lockoutOnFailure)
    {
        if (await _userManager.IsLockedOutAsync(user))
        {
            return SignInResult.LockedOut;
        }

        var success = await _userManager.CheckPasswordAsync(user, password);
        if (!success)
        {
            if (lockoutOnFailure)
            {
                var r = await AccessFailedAsync(user);
                if (!r.Succeeded)
                {
                    throw new ApplicationException("AccessFailed failed");
                }
            }

            return SignInResult.Failed;
        }

        return SignInResult.Success;
    }

    public async Task ConfirmPhoneNumberAsync(Guid userId)
    {
        User user = await FindByIdAsync(userId) ??
            throw new ArgumentException($"{userId}的用户不存在", nameof(userId));

        user.PhoneNumberConfirmed = true;
        await _userManager.UpdateAsync(user);
    }

    public Task<IdentityResult> CreateAsync(User user, string password)
    {
        return _userManager.CreateAsync(user, password);
    }

    public Task<User?> FindByIdAsync(Guid userId)
    {
        return _userManager.FindByIdAsync(userId.ToString());
    }

    public Task<User?> FindByPhoneNumberAsync(string phoneNum)
    {
        return _userManager.Users.SingleOrDefaultAsync(u => u.PhoneNumber == phoneNum);
    }

    public Task<string> GenerateChangePhoneNumberTokenAsync(User user, string phoneNumber)
    {
        return _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
    }

    public Task<IList<string>> GetRolesAsync(User user)
    {
        return _userManager.GetRolesAsync(user);
    }

    /// <summary>
    /// 软删除
    /// </summary>
    public async Task<IdentityResult> RemoveUserAsync(Guid userId)
    {
        User user = await FindByIdAsync(userId) ??
             throw new ArgumentException($"{userId}的用户不存在", nameof(userId));

        var userLoginStore = _userManager.UserLoginStore;
        var noneCT = default(CancellationToken);
        /*
         * 一定要删除aspnetuserlogins表中的数据，否则再次用这个外部登录登录的话
         * 就会报错：The instance of entity type 'IdentityUserLogin<Guid>' cannot be tracked 
         * because another instance with the same key value for {'LoginProvider', 'ProviderKey'} is already being tracked
         * 而且要先删除aspnetuserlogins数据，再软删除User
         */
        var logins = await userLoginStore.GetLoginsAsync(user, noneCT);
        foreach (var login in logins)
        {
            await userLoginStore.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey, noneCT);
        }
        user.SoftDelete();
        return await _userManager.UpdateAsync(user);
    }

    public async Task<(IdentityResult, User?, string? password)> ResetPasswordAsync(Guid id)
    {
        var user = await FindByIdAsync(id);
        if (user == null)
        {
            return (ErrorResult("用户没找到"), null, null);
        }
        string token = await _userManager.GeneratePasswordResetTokenAsync(user);

        string pwd = GeneratePassword();
        var result = await _userManager.ResetPasswordAsync(user, token, pwd);
        if (result == IdentityResult.Success)
        {
            return (IdentityResult.Success, user, pwd);
        }
        else
        {
            return (result, null, null);
        }
    }

    public async Task<IdentityResult> UpdatePhoneNumberAsync(Guid userId, string phoneNum)
    {
        User? user = await FindByIdAsync(userId);
        if (user == null)
        {
            return ErrorResult($"{userId}的用户不存在");
        }

        string token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNum);
        var result = await _userManager.ChangePhoneNumberAsync(user, phoneNum, token);
        if (!result.Succeeded)
        {
            string errMsg = result.Errors.SumErrors();
            _logger.LogError($"userId：{userId}，phoneNum：{phoneNum} UpdatePhoneNumberAsync失败，错误信息：{errMsg}");
        }

        return result;
    }
}
