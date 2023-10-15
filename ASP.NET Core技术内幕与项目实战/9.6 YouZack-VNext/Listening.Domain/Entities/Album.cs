using Zack.DomainCommons.Models;

namespace Listening.Domain.Entities;
/*
 * 分类，专辑，音频都是聚合根，而不像订单是聚合根，而订单明细是子实体一样
 * 因为订单和订单明细是整体和部分的关系，一起出现，订单明细不会单独访问，但是专辑，音频可以被单独访问
 */
/// <summary>
/// 专辑
/// </summary>
public record Album : AggregateRootEntity, IAggregateRoot
{
    private Album()
    {
        Name = new MultilingualString(string.Empty, string.Empty);
    }
    public Album(Guid id, MultilingualString name, int sequenceNumber, Guid categoryId)
    {
        Id = id;
        Name = name;
        SequenceNumber = sequenceNumber;
        CategoryId = categoryId;
        //Album新建以后默认不可见，需要手动Show
        IsVisible = false;
    }

    public static Album Create(Guid id, int sequenceNumber, MultilingualString name, Guid categoryId)
    {
        return new Album(id, name, sequenceNumber, categoryId);
    }

    /// <summary>
    /// 用户是否可见（完善后才显示，或者已经显示了，但是发现内部有问题，就先隐藏，调整了再发布）
    /// </summary>
    public bool IsVisible { get; private set; }

    /// <summary>
    /// 标题
    /// </summary>
    public MultilingualString Name { get; private set; }

    /// <summary>
    /// 列表中的显示序号
    /// </summary>
    public int SequenceNumber { get; private set; }

    public Guid CategoryId { get; private set; }

    public Album ChangeSequenceNumber(int value)
    {
        SequenceNumber = value;
        return this;
    }

    public Album ChangeName(MultilingualString value)
    {
        Name = value;
        return this;
    }
    public Album Hide()
    {
        IsVisible = false;
        return this;
    }
    public Album Show()
    {
        IsVisible = true;
        return this;
    }
}
