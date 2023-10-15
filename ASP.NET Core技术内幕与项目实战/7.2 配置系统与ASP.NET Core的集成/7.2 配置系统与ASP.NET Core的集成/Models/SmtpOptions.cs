namespace _7._2_配置系统与ASP.NET_Core的集成;

public record SmtpOptions
{
    public string? Host { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }
}
