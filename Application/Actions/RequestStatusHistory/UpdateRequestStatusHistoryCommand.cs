using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Actions.RequestStatusHistory;

public class UpdateRequestStatusHistoryCommand : IRequest<Result>
{
    public int EntityId { get; init; }
    public DateTime Date { get; init; }
    public int RepairRequestId { get; init; }
    public string RequestStatus { get; init; } = null!;
}

public class UpdateRequestStatusHistoryHandler : BaseHandler, IRequestHandler<UpdateRequestStatusHistoryCommand, Result>
{
    public UpdateRequestStatusHistoryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<Result> Handle(UpdateRequestStatusHistoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.RequestStatusHistories.FindAsync(request.EntityId);

        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.RequestStatusHistory),
                request.EntityId));
        }

        Mapper!.Map(request, entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class UpdateRequestStatusHistoryValidator : AbstractValidator<UpdateRequestStatusHistoryCommand>
{
    public UpdateRequestStatusHistoryValidator()
    {
        RuleFor(x => x.EntityId).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.RepairRequestId).NotEmpty();
        RuleFor(x => x.RequestStatus).NotEmpty().IsEnumName(typeof(RequestStatus));
    }
}