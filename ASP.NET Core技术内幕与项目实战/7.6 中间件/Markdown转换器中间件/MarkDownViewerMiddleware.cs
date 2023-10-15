using MarkdownSharp;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using Ude;

namespace Markdown转换器中间件;

public class MarkDownViewerMiddleware
{
    private readonly RequestDelegate next;
    private readonly IWebHostEnvironment hostEnv;
    private readonly IMemoryCache memCache;
    public MarkDownViewerMiddleware(RequestDelegate next, IWebHostEnvironment hostEnv, IMemoryCache memCache)
    {
        this.next = next;
        this.hostEnv = hostEnv;
        this.memCache = memCache;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        string path = context.Request.Path.Value ?? "";
        if (!path.EndsWith(".md"))
        {
            await next(context);
            return;
        }
        var file = hostEnv.WebRootFileProvider.GetFileInfo(path);
        if (!file.Exists)
        {
            await next(context);
            return;
        }
        context.Response.ContentType = "text/html;charset=UTF-8";
        context.Response.StatusCode = 200;
        string cacheKey = nameof(MarkDownViewerMiddleware) + path + file.LastModified;
        var html = await memCache.GetOrCreateAsync(cacheKey, async ce =>
        {
            Console.WriteLine("并未从缓存中获取到内容，正在获取...");
            ce.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
            using var stream = file.CreateReadStream();
            string text = await ReadText(stream);
            Markdown markdown = new Markdown();
            return markdown.Transform(text);
        });
        await context.Response.WriteAsync(html);
    }

    private static string DetectCharset(Stream stream)         //探测流的编码
    {
        CharsetDetector charDetector = new();
        charDetector.Feed(stream);
        charDetector.DataEnd();
        string charset = charDetector.Charset ?? "UTF-8";
        stream.Position = 0;
        return charset;
    }
    private static async Task<string> ReadText(Stream stream)   //从流中读取文本
    {
        string charset = DetectCharset(stream);
        using var reader = new StreamReader(stream, Encoding.GetEncoding(charset));
        return await reader.ReadToEndAsync();
    }
}


