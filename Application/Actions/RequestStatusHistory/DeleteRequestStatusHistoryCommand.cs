using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.RequestStatusHistory;

public class DeleteRequestStatusHistoryCommand : IRequest<Result>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class DeleteRequestStatusHistoryHandler : IRequestHandler<DeleteRequestStatusHistoryCommand, Result>
{
    public async Task<Result> Handle(DeleteRequestStatusHistoryCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }

        var entity = await request.DbContext.RequestStatusHistories.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(RequestStatusHistory), request.Id));
        }

        request.DbContext.RequestStatusHistories.Remove(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class DeleteRequestStatusHistoryValidator : AbstractValidator<DeleteRequestStatusHistoryCommand>
{
    public DeleteRequestStatusHistoryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}