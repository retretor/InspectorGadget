using FluentValidation;

namespace Application.Actions.RepairTypeForDevice.Validators;

public class DeleteRepairTypeForDeviceValidator : AbstractValidator<DeleteRepairTypeForDevice>
{
    public DeleteRepairTypeForDeviceValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}