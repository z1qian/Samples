using FileService.Domain;
using FileService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileService.Infratructure;

public class FSRepository : IFSRepository
{
    private readonly FSDBContext _dbContext;

    public FSRepository(FSDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<UploadedItem?> FindFileAsync(long fileSize, string sha256Hash)
    {
        return _dbContext.Query<UploadedItem>().FirstOrDefaultAsync(u => u.FileSizeInBytes == fileSize
            && u.FileSHA256Hash == sha256Hash);
    }
}
