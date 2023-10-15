using Users.Domain.Entities;

namespace Users.WebAPI.RequestParameters;

public record SendLoginByPhoneAndCodeRequest(PhoneNumber PhoneNumber);