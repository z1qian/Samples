namespace _8._4_SignalR服务器端消息推送;

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

