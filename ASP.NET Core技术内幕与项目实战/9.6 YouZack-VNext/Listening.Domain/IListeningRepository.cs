using Listening.Domain.Entities;

namespace Listening.Domain;

public interface IListeningRepository
{
    ValueTask<Category?> GetCategoryByIdAsync(Guid categoryId);
    Task<Category[]> GetCategoriesAsync();
    Task<int> GetMaxSeqOfCategoriesAsync();//获取最大序号
    ValueTask<Album?> GetAlbumByIdAsync(Guid albumId);
    Task<int> GetMaxSeqOfAlbumsAsync(Guid categoryId);
    Task<Album[]> GetAlbumsByCategoryIdAsync(Guid categoryId);
    Task<Episode?> GetEpisodeByIdAsync(Guid episodeId);
    Task<int> GetMaxSeqOfEpisodesAsync(Guid albumId);
    Task<Episode[]> GetEpisodesByAlbumIdAsync(Guid albumId);
}
