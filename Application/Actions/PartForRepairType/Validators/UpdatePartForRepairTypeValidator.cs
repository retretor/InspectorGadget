using FluentValidation;

namespace Application.Actions.PartForRepairType.Validators;

public class UpdatePartForRepairTypeValidator : AbstractValidator<UpdatePartForRepairType>
{
    public UpdatePartForRepairTypeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.PartCount).NotEmpty().GreaterThan(0);
        RuleFor(x => x.RepairTypeForDeviceId).NotEmpty();
        RuleFor(x => x.RepairPartId).NotEmpty();
    }
}