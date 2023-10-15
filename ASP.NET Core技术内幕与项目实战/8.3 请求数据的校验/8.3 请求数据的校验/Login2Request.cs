namespace _8._3_请求数据的校验;

public record Login2Request(string Email, string Password, string Password2);

public record Login3Request(string UserName, string Password);
