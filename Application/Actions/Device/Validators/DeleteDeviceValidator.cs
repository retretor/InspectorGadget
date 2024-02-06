using FluentValidation;

namespace Application.Actions.Device.Validators;

public class DeleteDeviceValidator : AbstractValidator<DeleteDevice>
{
    public DeleteDeviceValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}