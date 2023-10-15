namespace Zack.Commons;

public static class FormattableStringHelper
{
    /// <summary>
    /// 在Uri中将字符串转换为其转义表示形式
    public static string BuildUrl(FormattableString urlFormat)
    {
        var invariantParameters = urlFormat.GetArguments().Select(a => FormattableString.Invariant($"{a}"));
        object[] escapedParameters = invariantParameters.Select(s => (object)Uri.EscapeDataString(s)).ToArray();
        return string.Format(urlFormat.Format, escapedParameters);
    }
}
