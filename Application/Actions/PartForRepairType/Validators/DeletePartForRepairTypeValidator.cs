using FluentValidation;

namespace Application.Actions.PartForRepairType.Validators;

public class DeletePartForRepairTypeValidator : AbstractValidator<DeletePartForRepairType>
{
    public DeletePartForRepairTypeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}