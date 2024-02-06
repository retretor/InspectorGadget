using FluentValidation;

namespace Application.Actions.RequestStatusHistory.Validators;

public class UpdateRequestStatusHistoryValidator : AbstractValidator<UpdateRequestStatusHistory>
{
    public UpdateRequestStatusHistoryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.RepairRequestId).NotEmpty();
        RuleFor(x => x.RequestStatus).NotEmpty();
    }
}