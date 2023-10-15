namespace CommonInitializer;

//internal record CorsSettingOptions(string[] Origins);

public class CorsSettingOptions
{
    public string[] Origins { get; set; } = Array.Empty<string>();
}