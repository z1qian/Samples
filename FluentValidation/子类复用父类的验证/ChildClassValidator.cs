using FluentValidation;

namespace FluentValidationSample;

public class ChildClassValidator : AbstractValidator<ChildClass>
{
    public ChildClassValidator()
    {
        RuleFor(x => x.Id).GreaterThanOrEqualTo(1);

        Include(new ParentClassValidator());
    }
}