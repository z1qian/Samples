using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain;

public interface IIdRepository
{
    /// <summary>
    /// 根据UserId获取用户
    /// </summary>
    Task<User?> FindByIdAsync(Guid userId);
    /// <summary>
    /// 根据用户名获取用户
    /// </summary>
    Task<User?> FindByNameAsync(string userName);
    /// <summary>
    /// 根据手机号获取用户
    /// </summary>
    Task<User?> FindByPhoneNumberAsync(string phoneNum);
    /// <summary>
    /// 创建用户
    /// </summary>
    Task<IdentityResult> CreateAsync(User user, string password);
    /// <summary>
    /// 记录一次登陆失败
    /// </summary>
    Task<IdentityResult> AccessFailedAsync(User user);

    /// <summary>
    /// 生成重置密码的令牌
    /// </summary>
    Task<string> GenerateChangePhoneNumberTokenAsync(User user, string phoneNumber);
    /// <summary>
    /// 检查VCode，然后设置用户手机号为phoneNum
    /// </summary>
    Task<IdentityResult> ChangePhoneNumAsync(Guid userId, string phoneNum, string token);
    /// <summary>
    /// 修改密码
    /// </summary>
    Task<IdentityResult> ChangePasswordAsync(Guid userId, string password);

    /// <summary>
    /// 获取用户的角色
    /// </summary>
    Task<IList<string>> GetRolesAsync(User user);

    /// <summary>
    /// 把用户user加入角色role
    /// </summary>
    Task<IdentityResult> AddToRoleAsync(User user, string role);
    /// <summary>
    /// 为了登录而检查用户名、密码是否正确
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <param name="lockoutOnFailure">如果登录失败，则记录一次登陆失败</param>
    /// <returns></returns>
    Task<SignInResult> CheckForSignInAsync(User user, string password, bool lockoutOnFailure);
    /// <summary>
    /// 确认手机号
    /// </summary>
    Task ConfirmPhoneNumberAsync(Guid id);

    /// <summary>
    /// 修改手机号
    /// </summary>
    Task<IdentityResult> UpdatePhoneNumberAsync(Guid userId, string phoneNum);
    /// <summary>
    /// 删除用户
    /// </summary>
    Task<IdentityResult> RemoveUserAsync(Guid id);

    /// <summary>
    /// 添加管理员
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="phoneNum"></param>
    /// <returns>返回值第三个是生成的密码</returns>
    Task<(IdentityResult, User?, string? password)> AddAdminUserAsync(string userName, string phoneNum);

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="id"></param>
    /// <returns>返回值第三个是生成的密码</returns>
    Task<(IdentityResult, User?, string? password)> ResetPasswordAsync(Guid id);
}
