using FluentValidation;
using Listening.Domain.Entities;
using Listening.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Zack.DomainCommons.Models;

namespace Listening.Admin.WebAPI.Albums.Request;

public record AlbumAddRequest(MultilingualString Name, Guid CategoryId);

//把校验规则写到单独的文件，也是DDD的一种原则
public class AlbumAddRequestValidator : AbstractValidator<AlbumAddRequest>
{
    public AlbumAddRequestValidator(ListeningDbContext dbCtx)
    {
        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Name.Chinese).NotNull().Length(1, 200);
        RuleFor(x => x.Name.English).NotNull().Length(1, 200);
        ///验证CategoryId是否存在
        RuleFor(x => x.CategoryId).Must(cId =>
            dbCtx.Query<Category>().Any(c => c.Id == cId))
            .WithMessage(c => $"CategoryId={c.CategoryId}不存在");
    }
}