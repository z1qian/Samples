using Users.Domain.Entities;

namespace Users.WebAPI.RequestParameters
{
    public record CheckLoginByPhoneAndCodeRequest(PhoneNumber PhoneNumber, string Code);
}
