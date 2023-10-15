using Users.Domain.Entities;

namespace Users.WebAPI.RequestParameters;

public record LoginByPhoneAndPwdRequest(PhoneNumber PhoneNumber, string Password);
