namespace Listening.Domain.Subtitles;

interface ISubtitleParser
{
    /// <summary>
    /// 本解析器是否能够解析typeName这个类型的字幕
    /// </summary>
    bool Accept(string typeName);

    /// <summary>
    /// 解析这个字幕subtitle
    /// </summary>
    IEnumerable<Sentence>? Parse(string subtitle);
}
