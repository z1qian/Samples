using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;

var url = "http://www.some-api.com"
    .AppendPathSegment("endpoint")
    .SetQueryParams(new
    {
        api_key = "SomeApiKey",
        max_results = 20,
        q = "Don't worry, I'll get encoded!"
    })
    .SetFragment("after-hash");
Console.WriteLine(url);

Console.WriteLine(Url.Encode("http://foo.com?x=hi there", true)); // includes reserved characters like / and ?
Console.WriteLine(Url.EncodeIllegalCharacters("http://foo.com?x=hi there", false)); // reserved characters aren't touched
Console.WriteLine(Url.Decode("http%3A%2F%2Ffoo.com%3Fx%3Dhi there", false));


using (var session = new CookieSession("https://cookies.com"))
{
    // set any initial cookies on session.Cookies

    await session.Request("a").GetAsync();
    await session.Request("b").GetAsync();
    // read cookies at any point using session.Cookies
}

FlurlHttp.Configure(settings =>
{
    var jsonSettings = new JsonSerializerSettings
    {
        NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
        ObjectCreationHandling = ObjectCreationHandling.Replace
    };
    settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings);
});