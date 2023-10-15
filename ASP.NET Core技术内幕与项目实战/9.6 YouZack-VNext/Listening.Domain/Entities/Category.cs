using Zack.DomainCommons.Models;

namespace Listening.Domain.Entities;

/// <summary>
/// 分类
/// </summary>
public record Category : AggregateRootEntity, IAggregateRoot
{
    private Category()
    {
        Name = new MultilingualString(string.Empty, string.Empty);
        CoverUrl = new Uri("https://img-s-msn-com.akamaized.net/tenant/amp/entityid/AA1eFepO.img?w=768&h=439&m=6&x=1030&y=283&s=108&d=108");
    }

    public Category(Guid id, int sequenceNumber, MultilingualString name, Uri coverUrl)
    {
        Id = id;
        SequenceNumber = sequenceNumber;
        Name = name;
        CoverUrl = coverUrl;
    }

    public static Category Create(Guid id, int sequenceNumber, MultilingualString name, Uri coverUrl)
    {
        Category category = new(id, sequenceNumber, name, coverUrl);
        //category.AddDomainEvent(new CategoryCreatedEventArgs { NewObj = category });
        return category;
    }

    /// <summary>
    /// 在所有Category中的显示序号，越小越靠前
    /// </summary>
    public int SequenceNumber { get; private set; }
    public MultilingualString Name { get; private set; }

    /// <summary>
    /// 封面图片。现在一般都不会直接把图片保存到数据库中（Blob），而是只是保存图片的路径。
    /// </summary>
    public Uri CoverUrl { get; private set; }

    public Category ChangeSequenceNumber(int value)
    {
        SequenceNumber = value;
        return this;
    }

    public Category ChangeName(MultilingualString value)
    {
        Name = value;
        return this;
    }

    public Category ChangeCoverUrl(Uri value)
    {
        //todo: 做项目的时候，不管这个事件是否有被用到，都尽量publish。
        CoverUrl = value;
        return this;
    }
}
