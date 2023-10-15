using Users.Domain.Entities;
using Users.Domain.Events;

namespace Users.Domain;

public interface IUserDomainRepository
{
    /// <summary>
    /// 根据手机号查找用户
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    Task<User?> FindOneAsync(PhoneNumber phoneNumber);
    /// <summary>
    /// 根据用户ID查找用户
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<User?> FindOneAsync(Guid userId);
    /// <summary>
    /// 记录一次登录操作
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
    Task AddNewLoginHistoryAsync(PhoneNumber phoneNumber, string msg);
    /// <summary>
    /// 用于发布领域事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    Task PublishEventAsync(UserAccessResultEvent eventData);
    /// <summary>
    /// 用于保存短信验证码
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    Task SavePhoneCodeAsync(PhoneNumber phoneNumber, string code);
    /// <summary>
    /// 用于获取保存的短信验证码
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    Task<string?> RetrievePhoneCodeAsync(PhoneNumber phoneNumber);
}