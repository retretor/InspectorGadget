using FluentValidation;

namespace Application.Actions.Employee.Validators;

public class CreateEmployeeValidator : AbstractValidator<CreateEmployee>
{
    public CreateEmployeeValidator()
    {
        RuleFor(x => x.DbUserId).NotEmpty();
    }
}