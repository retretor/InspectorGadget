using FluentValidation;

namespace Application.Actions.RequestStatusHistory.Validators;

public class DeleteRequestStatusHistoryValidator : AbstractValidator<DeleteRequestStatusHistory>
{
    public DeleteRequestStatusHistoryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}