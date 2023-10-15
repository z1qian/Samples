namespace 标识框架的使用.Models;

public record VerifyResetPasswordTokenRequest(string Email, string Token, string NewPassword);