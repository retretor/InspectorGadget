using FluentValidation;

namespace Application.Actions.RepairRequest.Validators;

public class CreateRepairRequestValidator : AbstractValidator<CreateRepairRequest>
{
    public CreateRepairRequestValidator()
    {
        RuleFor(x => x.DeviceId).NotEmpty();
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.SerialNumber).NotEmpty().MaximumLength(20);
    }
}