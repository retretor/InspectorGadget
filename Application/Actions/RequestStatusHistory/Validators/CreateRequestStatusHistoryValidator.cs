using FluentValidation;

namespace Application.Actions.RequestStatusHistory.Validators;

public class CreateRequestStatusHistoryValidator : AbstractValidator<CreateRequestStatusHistory>
{
    public CreateRequestStatusHistoryValidator()
    {
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.RepairRequestId).NotEmpty();
        RuleFor(x => x.RequestStatus).NotEmpty();
    }
}