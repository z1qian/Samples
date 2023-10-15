using Listening.Domain.Entities;
using Listening.Domain.Subtitles;
using Zack.DomainCommons.Models;
namespace Listening.Main.WebAPI.Episodes;

public record EpisodeVM(Guid Id, MultilingualString Name, Guid AlbumId, Uri AudioUrl, double DurationInSecond, IEnumerable<SentenceVM>? Sentences)
{
    public static EpisodeVM? Create(Episode? e, bool loadSubtitle)
    {
        if (e == null)
        {
            return null;
        }
        List<SentenceVM> sentenceVMs = new();
        if (loadSubtitle)
        {
            var sentences = e.ParseSubtitle();
            foreach (Sentence s in sentences)
            {
                SentenceVM vm = new SentenceVM(s.StartTime.TotalSeconds, s.EndTime.TotalSeconds, s.Content);
                sentenceVMs.Add(vm);
            }
        }
        return new EpisodeVM(e.Id, e.Name, e.AlbumId, e.AudioUrl, e.DurationInSecond, sentenceVMs);
    }

    public static EpisodeVM[] Create(Episode[] items, bool loadSubtitle)
    {
        return items.Select(e => Create(e, loadSubtitle)!).ToArray();
    }
}
