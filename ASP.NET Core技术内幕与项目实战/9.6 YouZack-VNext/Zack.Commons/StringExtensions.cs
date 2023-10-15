namespace System;

public static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string s1, string s2)
    {
        return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 截取字符串s1最多前maxLen个字符
    /// </summary>
    public static string Cut(this string s1, int maxLen)
    {
        if (string.IsNullOrWhiteSpace(s1))
        {
            return string.Empty;
        }
        int len = s1.Length <= maxLen ? s1.Length : maxLen;//不能超过字符串的最大大小
        return s1[0..len];
    }
}
