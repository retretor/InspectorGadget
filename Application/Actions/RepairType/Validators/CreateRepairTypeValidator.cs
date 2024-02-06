using FluentValidation;

namespace Application.Actions.RepairType.Validators;

public class CreateRepairTypeValidator : AbstractValidator<CreateRepairType>
{
    public CreateRepairTypeValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}