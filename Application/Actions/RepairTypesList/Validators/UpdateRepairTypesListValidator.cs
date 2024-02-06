using FluentValidation;

namespace Application.Actions.RepairTypesList.Validators;

public class UpdateRepairTypesListValidator : AbstractValidator<UpdateRepairTypesList>
{
    public UpdateRepairTypesListValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RepairTypeForDeviceId).NotEmpty();
        RuleFor(x => x.RepairRequestId).NotEmpty();
    }
}