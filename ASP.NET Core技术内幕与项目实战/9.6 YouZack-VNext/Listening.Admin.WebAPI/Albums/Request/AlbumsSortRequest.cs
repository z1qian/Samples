using FluentValidation;

namespace Listening.Admin.WebAPI.Albums.Request;
public record AlbumsSortRequest(Guid[] SortedAlbumIds);

public class AlbumsSortRequestValidator : AbstractValidator<AlbumsSortRequest>
{
    public AlbumsSortRequestValidator()
    {
        RuleFor(r => r.SortedAlbumIds).NotNull().NotEmpty().NotContains(Guid.Empty)
            .NotDuplicated();
    }
}
