using FluentValidation;

namespace Application.Actions.RepairTypeForDevice.Validators;

public class UpdateRepairTypeForDeviceValidator : AbstractValidator<UpdateRepairTypeForDevice>
{
    public UpdateRepairTypeForDeviceValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Cost).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Time).NotEmpty().GreaterThan(0);
        RuleFor(x => x.RepairTypeId).NotEmpty();
        RuleFor(x => x.DeviceId).NotEmpty();
    }
}