namespace _4._6_关系配置;

/// <summary>
/// 文章实体类
/// </summary>
internal class Article
{
    public Article()
    {
        Comments = new List<Comment>();
    }
    public long Id { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    //此文章的多条评论
    public List<Comment>? Comments { get; set; }
}


