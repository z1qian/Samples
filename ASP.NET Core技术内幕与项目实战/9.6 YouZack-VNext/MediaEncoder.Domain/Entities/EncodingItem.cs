using MediaEncoder.Domain.EventDatas;
using Zack.DomainCommons.Models;

namespace MediaEncoder.Domain.Entities;

public record EncodingItem : BaseEntity, IAggregateRoot, IHasCreationTime
{
    public EncodingItem(Guid id, string sourceSystem, string name, Uri sourceUrl, string outputFormat)
    {
        Id = id;
        CreationTime = DateTime.Now;
        SourceSystem = sourceSystem;
        FileSizeInBytes = 0;
        Name = name;
        FileSHA256Hash = string.Empty;
        SourceUrl = sourceUrl;
        OutputFormat = outputFormat;
        Status = ItemStatus.Ready;
    }

    public DateTime CreationTime { get; private set; }
    public string SourceSystem { get; private set; }

    /// <summary>
    /// 文件大小（尺寸为字节）
    /// </summary>
    public long FileSizeInBytes { get; private set; }
    /// <summary>
    /// 文件名字（非全路径）
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 两个文件的大小和散列值（SHA256）都相同的概率非常小。因此只要大小和SHA256相同，就认为是相同的文件。
    /// SHA256的碰撞的概率比MD5低很多。
    /// </summary>
    public string FileSHA256Hash { get; private set; }

    /// <summary>
    /// 待转码的文件
    /// </summary>
    public Uri SourceUrl { get; private set; }


    /// <summary>
    /// 转码完成的路径
    /// </summary>
    public Uri? OutputUrl { get; private set; }

    /// <summary>
    /// 转码目标类型，比如m4a、mp4等
    /// </summary>
    public string OutputFormat { get; private set; }

    public ItemStatus Status { get; private set; }

    /// <summary>
    /// 转码工具的输出日志
    /// </summary>
    public string? LogText { get; private set; }

    public void Start()
    {
        Status = ItemStatus.Started;
        AddDomainEvent(new EncodingItemStartedEventData(Id, SourceSystem));
    }

    public void Complete(Uri outputUrl)
    {
        Status = ItemStatus.Completed;
        OutputUrl = outputUrl;
        LogText = "转码成功";
        AddDomainEvent(new EncodingItemCompletedEventData(Id, SourceSystem, outputUrl));
    }

    public void Fail(string logText)
    {
        //todo：通过集成事件写入Logging系统
        Status = ItemStatus.Failed;
        LogText = logText;
        AddDomainEventIfAbsent(new EncodingItemFailedEventData(Id, SourceSystem, logText));
    }

    public void Fail(Exception ex)
    {
        Fail($"转码处理失败：{ex}");
    }

    public void ChangeFileMeta(long fileSize, string hash)
    {
        FileSizeInBytes = fileSize;
        FileSHA256Hash = hash;
    }

    public static EncodingItem Create(Guid id, string name, Uri sourceUrl, string outputFormat, string sourceSystem)
    {
        EncodingItem item = new EncodingItem(id, sourceSystem, name, sourceUrl, outputFormat);
        item.AddDomainEvent(new EncodingItemCreatedEventData(item));
        return item;
    }
}