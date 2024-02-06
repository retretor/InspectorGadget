using FluentValidation;

namespace Application.Actions.RepairPart.Validators;

public class CreateRepairPartValidator : AbstractValidator<CreateRepairPart>
{
    public CreateRepairPartValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Specification).NotEmpty();
        RuleFor(x => x.CurrentCount).NotEmpty().GreaterThanOrEqualTo(0);
        RuleFor(x => x.MinAllowedCount).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Cost).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Condition).NotEmpty();
    }
}