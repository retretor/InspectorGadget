using FluentValidation;

namespace Application.Actions.RepairTypesList.Validators;

public class DeleteRepairTypesListValidator : AbstractValidator<DeleteRepairTypesList>
{
    public DeleteRepairTypesListValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}