using FluentValidation;

namespace Application.Actions.RepairTypesList.Validators;

public class CreateRepairTypesListValidator : AbstractValidator<CreateRepairTypesList>
{
    public CreateRepairTypesListValidator()
    {
        RuleFor(x => x.RepairTypeForDeviceId).NotEmpty();
        RuleFor(x => x.RepairRequestId).NotEmpty();
    }
}