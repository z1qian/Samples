namespace IdentityService.WebAPI.Events;

public record UserCreatedEventData(Guid Id, string UserName, string Password, string PhoneNum);
