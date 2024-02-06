using FluentValidation;

namespace Application.Actions.RepairPart.Validators;

public class DeleteRepairPartValidator : AbstractValidator<DeleteRepairPart>
{
    public DeleteRepairPartValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}