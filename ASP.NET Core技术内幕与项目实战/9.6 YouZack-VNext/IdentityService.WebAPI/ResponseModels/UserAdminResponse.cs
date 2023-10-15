using IdentityService.Domain.Entities;

namespace IdentityService.WebAPI.ResponseModels;

public record UserAdminResponse(Guid Id, string? UserName, string? PhoneNumber, DateTime CreationTime)
{
    public static UserAdminResponse Create(User user)
    {
        return new UserAdminResponse(user.Id, user.UserName, user.PhoneNumber, user.CreationTime);
    }
}
