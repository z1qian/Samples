namespace CommonInitializer;

public class ThrowHelper
{
    public static void ThorwArgumentNullExceptionIfNullOrWhiteSpace(string? content, string paramName, string msg)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentNullException(paramName, msg);
        }
    }
}
