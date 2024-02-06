using FluentValidation;

namespace Application.Actions.Employee.Validators;

public class DeleteEmployeeValidator : AbstractValidator<DeleteEmployee>
{
    public DeleteEmployeeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}