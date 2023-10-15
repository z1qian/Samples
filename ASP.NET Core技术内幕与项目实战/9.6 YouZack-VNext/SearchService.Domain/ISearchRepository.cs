namespace SearchService.Domain;

public interface ISearchRepository
{
    Task UpsertAsync(Episode episode);
    Task DeleteAsync(Guid episodeId);
    Task<SearchEpisodesResponse> SearchEpisodes(string keyWord, int pageIndex, int PageSize);
}