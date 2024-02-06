using FluentValidation;

namespace Application.Actions.PartForRepairType.Validators;

public class CreatePartForRepairTypeValidator : AbstractValidator<CreatePartForRepairType>
{
    public CreatePartForRepairTypeValidator()
    {
        RuleFor(x => x.PartCount).NotEmpty().GreaterThan(0);
        RuleFor(x => x.RepairTypeForDeviceId).NotEmpty();
        RuleFor(x => x.RepairPartId).NotEmpty();
    }
}