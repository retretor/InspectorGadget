using FluentValidation;

namespace Application.Actions.RepairType.Validators;

public class DeleteRepairTypeValidator : AbstractValidator<DeleteRepairType>
{
    public DeleteRepairTypeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}