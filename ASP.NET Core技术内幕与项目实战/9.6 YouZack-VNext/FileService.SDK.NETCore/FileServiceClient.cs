using System.Security.Claims;
using Zack.Commons;
using Zack.JWT;

namespace FileService.SDK.NETCore;

public class FileServiceClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Uri _serverRoot;
    private readonly JWTOptions _optionsSnapshot;
    private readonly ITokenService _tokenService;

    public FileServiceClient(IHttpClientFactory httpClientFactory, Uri serverRoot, JWTOptions optionsSnapshot, ITokenService tokenService)
    {
        _httpClientFactory = httpClientFactory;
        _serverRoot = serverRoot;
        _optionsSnapshot = optionsSnapshot;
        _tokenService = tokenService;
    }

    public Task<FileExistsResponse?> FileExistsAsync(long fileSize, string sha256Hash,
        CancellationToken cancellationToken = default)
    {
        string token = BuildToken();
        string relativeUrl = FormattableStringHelper.BuildUrl($"Uploader/FileExists?fileSize={fileSize}&sha256Hash={sha256Hash}");
        Uri requestUrl = new(_serverRoot, relativeUrl);
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return httpClient.GetJsonAsync<FileExistsResponse>(requestUrl, cancellationToken);
    }

    public async Task<Uri> UploadAsync(FileInfo file, CancellationToken stoppingToken = default)
    {
        string token = BuildToken();
        // Uri requestUri = new(_serverRoot, "/Uploader/Upload");
        Uri requestUri = new($"{_serverRoot.OriginalString}/Uploader/Upload");

        using MultipartFormDataContent content = new();

        using var fileContent = new StreamContent(file.OpenRead());

        content.Add(fileContent, "file", file.Name);

        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var respMsg = await httpClient.PostAsync(requestUri, content, stoppingToken);
        string respString = await respMsg.Content.ReadAsStringAsync(stoppingToken);
        if (!respMsg.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"上传失败，状态码：{respMsg.StatusCode}，响应报文：{respString}");
        }

        return respString.ParseJson<Uri>()!;
    }

    private string BuildToken()
    {
        Claim claim = new(ClaimTypes.Role, "Admin");
        return _tokenService.BuildToken(new Claim[] { claim }, _optionsSnapshot);
    }

}
