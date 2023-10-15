using FluentValidation;

namespace Listening.Admin.WebAPI.Categories.Request;

/// <summary>
/// 
/// </summary>
/// <param name="SortedCategoryIds">排序后的类别Id</param>
public record CategoriesSortRequest(Guid[] SortedCategoryIds);

public class CategoriesSortRequestValidator : AbstractValidator<CategoriesSortRequest>
{
    public CategoriesSortRequestValidator()
    {
        RuleFor(r => r.SortedCategoryIds).NotNull().NotEmpty().NotContains(Guid.Empty).NotDuplicated();
    }
}