using Users.Domain.Entities;

namespace Users.Domain;

public interface ISmsCodeSender
{
    Task SendCodeAsync(PhoneNumber phoneNumber, string code);
}