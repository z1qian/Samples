using FluentValidation;

namespace Listening.Admin.WebAPI.Episodes.Request;
public record EpisodesSortRequest(Guid[] SortedEpisodeIds);

public class EpisodesSortRequestValidator : AbstractValidator<EpisodesSortRequest>
{
    public EpisodesSortRequestValidator()
    {
        RuleFor(r => r.SortedEpisodeIds).NotNull().NotEmpty().NotContains(Guid.Empty).NotDuplicated();
    }
}
