using FluentValidation;

namespace Application.Actions.Device.Validators;

public class CreateDeviceValidator : AbstractValidator<CreateDevice>
{
    public CreateDeviceValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Type).NotEmpty();
        RuleFor(x => x.Brand).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Series).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Manufacturer).NotEmpty().MaximumLength(255);
    }
}