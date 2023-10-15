namespace IdentityService.WebAPI.Events;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="UserName"></param>
/// <param name="Password"></param>
/// <param name="PhoneNum">我们在此假定用户已填写了手机号，所以不能为空</param>
public record ResetPasswordEventData(Guid Id, string UserName, string Password, string PhoneNum);
