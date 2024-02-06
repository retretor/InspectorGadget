using FluentValidation;

namespace Application.Actions.RepairRequest.Validators;

public class DeleteRepairRequestValidator : AbstractValidator<DeleteRepairRequest>
{
    public DeleteRepairRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}