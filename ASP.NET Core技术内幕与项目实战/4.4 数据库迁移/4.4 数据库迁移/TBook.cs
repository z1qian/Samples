namespace _4._4_数据库迁移;

public partial class TBook
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime PubTime { get; set; }

    public double Price { get; set; }

    public string AuthorName { get; set; } = null!;

    public string Remark { get; set; } = null!;
}
