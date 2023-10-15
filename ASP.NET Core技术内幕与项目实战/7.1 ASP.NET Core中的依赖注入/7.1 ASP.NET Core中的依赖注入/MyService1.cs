namespace _7._1_ASP.NET_Core中的依赖注入;

public class MyService1
{
    public IEnumerable<string> GetNames()
    {
        return new string[] { "Tom", "Zack", "Jack" };
    }
}
