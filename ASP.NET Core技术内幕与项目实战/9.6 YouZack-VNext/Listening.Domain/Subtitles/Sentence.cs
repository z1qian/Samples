namespace Listening.Domain.Subtitles;

/// <summary>
/// 句子
/// </summary>
public record Sentence(TimeSpan StartTime, TimeSpan EndTime, string Content);
