namespace _4._6_关系配置;

/// <summary>
/// 评论实体类
/// </summary>
internal class Comment
{
    public long Id { get; set; }

    //评论属于哪篇文章
    public Article? Article { get; set; }

    public string? Message { get; set; }

    public long ArticleId { get; set; }
}
