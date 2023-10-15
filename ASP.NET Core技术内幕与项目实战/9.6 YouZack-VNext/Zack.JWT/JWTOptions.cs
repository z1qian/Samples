namespace Zack.JWT;

public class JWTOptions
{
    public JWTOptions()
    {
        Issuer = string.Empty;
        Audience = string.Empty;
        Key = string.Empty;
        ExpireSeconds = 0;
    }
    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string Key { get; set; }

    public int ExpireSeconds { get; set; }
}
