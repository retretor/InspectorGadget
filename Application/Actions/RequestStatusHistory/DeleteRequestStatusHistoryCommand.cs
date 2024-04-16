using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.RequestStatusHistory;

public class DeleteRequestStatusHistoryCommand : IRequest<Result>
{
    public int Id { get; init; }
}

public class DeleteRequestStatusHistoryHandler : BaseHandler, IRequestHandler<DeleteRequestStatusHistoryCommand, Result>
{
    public DeleteRequestStatusHistoryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result> Handle(DeleteRequestStatusHistoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.RequestStatusHistories.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(RequestStatusHistory), request.Id));
        }

        DbContext.RequestStatusHistories.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);

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