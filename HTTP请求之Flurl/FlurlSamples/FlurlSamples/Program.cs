using Flurl;

//var url = "http://www.some-api.com"
//    .AppendPathSegment("endpoint")
//    .SetQueryParams(new
//    {
//        api_key = "SomeApiKey",
//        max_results = 20,
//        q = "Don't worry, I'll get encoded!"
//    })
//    .SetFragment("after-hash");
//Console.WriteLine(url);

Console.WriteLine(Url.Encode("http://foo.com?x=hi there",true)); // includes reserved characters like / and ?
Console.WriteLine(Url.EncodeIllegalCharacters("http://foo.com?x=hi there", false)); ; // reserved characters aren't touched
Console.WriteLine(Url.Decode("http%3A%2F%2Ffoo.com%3Fx%3Dhi there", false));