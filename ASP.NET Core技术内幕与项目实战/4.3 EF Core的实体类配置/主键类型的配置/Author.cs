namespace 主键类型的配置;

internal class Author
{
    //使用GUID作为主键类型
    public Guid Id { get; set; }

    public string? Name { get; set; }
}
