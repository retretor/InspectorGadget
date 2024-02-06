using FluentValidation;

namespace Application.Actions.RepairType.Validators;

public class UpdateRepairTypeValidator : AbstractValidator<UpdateRepairType>
{
    public UpdateRepairTypeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}