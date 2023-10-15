namespace IdentityService.Domain;

public interface ISmsSender
{
    Task SendAsync(string phoneNum, params string[] args);
}
