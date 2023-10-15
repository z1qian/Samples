namespace Zack.DomainCommons.Models;

public interface ISoftDelete
{
    bool IsDeleted { get; }
    void SoftDelete();
}
