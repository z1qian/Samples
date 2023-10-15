namespace ASP.NET_Core中使用JWT.Models;

public class JWTOptions
{
    /// <summary>
    /// 密钥
    /// </summary>
    public string SigningKey { get; set; }

    /// <summary>
    /// 过期时间（单位秒）
    /// </summary>
    public int ExpireSeconds { get; set; }
}
