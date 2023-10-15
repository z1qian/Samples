using Listening.Domain;
using Listening.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Listening.Infrastructure;

public class ListeningRepository : IListeningRepository
{
    private readonly ListeningDbContext _dbCtx;

    public ListeningRepository(ListeningDbContext dbCtx)
    {
        _dbCtx = dbCtx;
    }

    public ValueTask<Category?> GetCategoryByIdAsync(Guid categoryId)
    {
        return _dbCtx.FindAsync<Category>(categoryId);
    }

    public Task<Category[]> GetCategoriesAsync()
    {
        return _dbCtx.Categories.OrderBy(e => e.SequenceNumber).ToArrayAsync();
    }

    public ValueTask<Album?> GetAlbumByIdAsync(Guid albumId)
    {
        return _dbCtx.FindAsync<Album>(albumId);
    }

    public async Task<int> GetMaxSeqOfAlbumsAsync(Guid categoryId)
    {
        //MaxAsync(c => (int?)c.SequenceNumber) 这样可以处理一条数据都没有的问题
        int? maxSeq = await _dbCtx.Query<Album>()
            .Where(c => c.CategoryId == categoryId)
            .MaxAsync(c => (int?)c.SequenceNumber);
        return maxSeq ?? 0;
    }

    public Task<Album[]> GetAlbumsByCategoryIdAsync(Guid categoryId)
    {
        return _dbCtx.Albums.OrderBy(e => e.SequenceNumber).Where(a => a.CategoryId == categoryId).ToArrayAsync();
    }

    public async Task<int> GetMaxSeqOfCategoriesAsync()
    {
        int? maxSeq = await _dbCtx.Query<Category>().MaxAsync(c => (int?)c.SequenceNumber);
        return maxSeq ?? 0;
    }

    public Task<Episode?> GetEpisodeByIdAsync(Guid episodeId)
    {
        return _dbCtx.Episodes.SingleOrDefaultAsync(e => e.Id == episodeId);
    }

    public async Task<int> GetMaxSeqOfEpisodesAsync(Guid albumId)
    {
        int? maxSeq = await _dbCtx.Query<Episode>().Where(e => e.AlbumId == albumId)
            .MaxAsync(e => (int?)e.SequenceNumber);
        return maxSeq ?? 0;
    }

    public Task<Episode[]> GetEpisodesByAlbumIdAsync(Guid albumId)
    {
        return _dbCtx.Episodes.OrderBy(e => e.SequenceNumber).Where(a => a.AlbumId == albumId).ToArrayAsync();
    }
}
