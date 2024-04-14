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
    public IApplicationDbContext? DbContext { get; set; }
}

public class
    IsAvailableAllPartsForRepairRequestHandler : IRequestHandler<IsAvailableAllPartsForRepairRequestQuery,
    (Result, bool?)>
{
    public async Task<(Result, bool?)> Handle(IsAvailableAllPartsForRepairRequestQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entity = await Task.Run(() =>
            request.DbContext.IsAvailableAllParts(request.EntityId).SingleOrDefaultAsync());
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