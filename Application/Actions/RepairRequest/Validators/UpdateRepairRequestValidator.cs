using FluentValidation;

namespace Application.Actions.RepairRequest.Validators;

public class UpdateRepairRequestValidator : AbstractValidator<UpdateRepairRequest>
{
    public UpdateRepairRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DeviceId).NotEmpty();
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.SerialNumber).NotEmpty().MaximumLength(20);
    }
}