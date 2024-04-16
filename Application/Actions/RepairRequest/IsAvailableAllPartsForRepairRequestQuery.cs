using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.RepairRequest;

public class IsAvailableAllPartsForRepairRequestQuery : IRequest<(Result, bool?)>
{
    public int EntityId { get; init; }
}

public class
    IsAvailableAllPartsForRepairRequestHandler : BaseHandler, IRequestHandler<IsAvailableAllPartsForRepairRequestQuery,
    (Result, bool?)>
{
    public IsAvailableAllPartsForRepairRequestHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, bool?)> Handle(IsAvailableAllPartsForRepairRequestQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await Task.Run(() =>
            DbContext.IsAvailableAllParts(request.EntityId).SingleOrDefaultAsync());
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(RepairRequest), request.EntityId)), null)
            : (Result.Success(), entity.Result);
    }
}

public class IsAvailableAllPartsForRepairRequestValidator : AbstractValidator<IsAvailableAllPartsForRepairRequestQuery>
{
    public IsAvailableAllPartsForRepairRequestValidator()
    {
        // TODO: check all validators of EntityId
        RuleFor(v => v.EntityId).NotEmpty();
    }
}