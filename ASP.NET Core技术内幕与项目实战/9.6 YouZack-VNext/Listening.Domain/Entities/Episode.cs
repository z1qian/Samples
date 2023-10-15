using Listening.Domain.EventDatas;
using Listening.Domain.Subtitles;
using Zack.DomainCommons.Models;

namespace Listening.Domain.Entities;
# pragma warning disable CS8618
public record Episode : AggregateRootEntity, IAggregateRoot
{
    private Episode() { }

    /// <summary>
    /// 序号
    /// </summary>
    public int SequenceNumber { get; private set; }

    /// <summary>
    /// 标题
    /// </summary>
    public MultilingualString Name { get; private set; }

    /// <summary>
    /// 专辑Id，因为Episode和Album都是聚合根，因此不能直接做对象引用
    /// </summary>
    public Guid AlbumId { get; private set; }

    /// <summary>
    /// 音频路径
    /// </summary>
    public Uri AudioUrl { get; private set; }

    /// <summary>
    /// 音频时长（秒数）（用于前端做比例校正）
    /// </summary>
    public double DurationInSecond { get; private set; }

    /// <summary>
    /// 原文字幕内容
    /// </summary>
    public string Subtitle { get; private set; }

    /// <summary>
    /// 原文字幕格式
    /// </summary>
    public string SubtitleType { get; private set; }

    /// <summary>
    /// 用户是否可见（如果发现内部有问题，就先隐藏）
    /// </summary>
    public bool IsVisible { get; private set; }

    public Episode ChangeSequenceNumber(int value)
    {
        SequenceNumber = value;
        AddDomainEventIfAbsent(new EpisodeUpdatedEventData(this));
        return this;
    }

    public Episode ChangeName(MultilingualString value)
    {
        Name = value;
        AddDomainEventIfAbsent(new EpisodeUpdatedEventData(this));
        return this;
    }

    public Episode ChangeSubtitle(string subtitleType, string subtitle)
    {
        var parser = SubtitleParserFactory.GetParser(subtitleType)
            ?? throw new ArgumentOutOfRangeException(nameof(subtitleType), $"subtitleType={subtitleType} is not supported.");

        SubtitleType = subtitleType;
        Subtitle = subtitle;
        AddDomainEventIfAbsent(new EpisodeUpdatedEventData(this));
        return this;
    }

    public Episode Hide()
    {
        IsVisible = false;
        AddDomainEventIfAbsent(new EpisodeUpdatedEventData(this));
        return this;
    }
    public Episode Show()
    {
        IsVisible = true;
        AddDomainEventIfAbsent(new EpisodeUpdatedEventData(this));
        return this;
    }

    public override void SoftDelete()
    {
        base.SoftDelete();
        AddDomainEvent(new EpisodeDeletedEventData(Id));
    }

    /// <summary>
    /// 字幕解析
    /// </summary>
    /// <returns>返回句子列表</returns>
    public IEnumerable<Sentence> ParseSubtitle()
    {
        var parser = SubtitleParserFactory.GetParser(SubtitleType);
        return parser?.Parse(Subtitle) ?? Enumerable.Empty<Sentence>();
    }

    public class Builder
    {
        private Guid id;
        private int sequenceNumber;
        private MultilingualString name;
        private Guid albumId;
        private Uri audioUrl;
        private double durationInSecond;
        private string subtitle;
        private string subtitleType;

        public Builder Id(Guid value)
        {
            id = value;
            return this;
        }
        public Builder SequenceNumber(int value)
        {
            sequenceNumber = value;
            return this;
        }
        public Builder Name(MultilingualString value)
        {
            name = value;
            return this;
        }
        public Builder AlbumId(Guid value)
        {
            albumId = value;
            return this;
        }
        public Builder AudioUrl(Uri value)
        {
            audioUrl = value;
            return this;
        }
        public Builder DurationInSecond(double value)
        {
            durationInSecond = value;
            return this;
        }
        public Builder Subtitle(string value)
        {
            subtitle = value;
            return this;
        }
        public Builder SubtitleType(string value)
        {
            subtitleType = value;
            return this;
        }
        public Episode Build()
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (albumId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }
            if (audioUrl == null)
            {
                throw new ArgumentNullException(nameof(audioUrl));
            }
            if (durationInSecond <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(durationInSecond));
            }
            if (subtitle == null)
            {
                throw new ArgumentNullException(nameof(subtitle));
            }
            if (subtitleType == null)
            {
                throw new ArgumentNullException(nameof(subtitleType));
            }
            Episode e = new Episode();
            e.Id = id;
            e.SequenceNumber = sequenceNumber;
            e.Name = name;
            e.AlbumId = albumId;
            e.AudioUrl = audioUrl;
            e.DurationInSecond = durationInSecond;
            e.Subtitle = subtitle;
            e.SubtitleType = subtitleType;
            e.IsVisible = true;
            e.AddDomainEvent(new EpisodeCreatedEventData(e));
            return e;
        }
    }
}
# pragma warning  restore CS8618
