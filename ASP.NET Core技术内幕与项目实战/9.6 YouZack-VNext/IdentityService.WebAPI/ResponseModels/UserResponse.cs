namespace IdentityService.WebAPI.ResponseModels;

public record UserResponse(Guid Id, string? PhoneNumber, DateTime CreationTime);
