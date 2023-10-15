using MediaEncoder.Domain;
using MediaEncoder.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediaEncoder.Infrastructure;

class MediaEncoderRepository : IMediaEncoderRepository
{
    private readonly MEDbContext _dbContext;

    public MediaEncoderRepository(MEDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<EncodingItem[]> FindAsync(ItemStatus status)
    {
        return _dbContext.EncodingItems.Where(e => e.Status == ItemStatus.Ready).ToArrayAsync();
    }

    public Task<EncodingItem?> FindCompletedOneAsync(string fileHash, long fileSize)
    {
        return _dbContext.EncodingItems.FirstOrDefaultAsync(e => e.FileSHA256Hash == fileHash
                && e.FileSizeInBytes == fileSize && e.Status == ItemStatus.Completed);
    }
}