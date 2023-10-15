using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Zack.Commons;

namespace System;

public static class JsonExtentions
{
    //如果不设置这个，那么"雅思真题"就会保存为"\u96C5\u601D\u771F\u9898"
    private static readonly JavaScriptEncoder Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);

    private static JsonSerializerOptions CreateJsonSerializerOptions(bool camelCase = false)
    {
        JsonSerializerOptions opt = new() { Encoder = Encoder };
        if (camelCase)
        {
            opt.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            opt.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        }
        opt.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));
        return opt;
    }

    public static string ToJsonString(this object value, bool camelCase = false)
    {
        JsonSerializerOptions opt = CreateJsonSerializerOptions(camelCase);
        return JsonSerializer.Serialize(value, value.GetType(), opt);
    }

    public static T? ParseJson<T>(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }
        JsonSerializerOptions opt = CreateJsonSerializerOptions();
        return JsonSerializer.Deserialize<T?>(value, opt);
    }
}
