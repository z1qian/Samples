using System.Security.Cryptography;
using System.Text;

namespace Zack.Commons;

public static class HashHelper
{
    private static string ToHashString(byte[] bytes)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
    }

    /// <summary>
    /// 计算Sha256Hash值
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static string ComputeSha256Hash(Stream stream)
    {
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(stream);
        return ToHashString(bytes);
    }

    /// <summary>
    /// 计算Sha256Hash值
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ComputeSha256Hash(string input)
    {
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        return ToHashString(bytes);
    }

    /// <summary>
    /// 计算MD5值
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ComputeMd5Hash(string input)
    {
        using MD5 md5Hash = MD5.Create();
        byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        return ToHashString(bytes);
    }

    /// <summary>
    /// 计算MD5值
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ComputeMd5Hash(Stream input)
    {
        using MD5 md5Hash = MD5.Create();
        byte[] bytes = md5Hash.ComputeHash(input);
        return ToHashString(bytes);
    }
}
