namespace Users.Domain.Entities;

/// <summary>
/// 
/// </summary>
/// <param name="RegionCode">区域代码</param>
/// <param name="Number">手机号</param>
public record PhoneNumber(int RegionCode, string Number);
