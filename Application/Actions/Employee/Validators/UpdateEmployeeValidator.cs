using FluentValidation;

namespace Application.Actions.Employee.Validators;

public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployee>
{
    public UpdateEmployeeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DbUserId).NotEmpty();
    }
}