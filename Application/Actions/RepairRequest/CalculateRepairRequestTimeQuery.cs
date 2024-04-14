using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.RepairRequest;

public class CalculateRepairRequestTimeQuery : IRequest<(Result, int?)>
{
    public int EntityId { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class
    CalculateRepairRequestTimeHandler : IRequestHandler<CalculateRepairRequestTimeQuery, (Result, int?)>
{
    public async Task<(Result, int?)> Handle(CalculateRepairRequestTimeQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entity = await Task.Run(() =>
            request.DbContext.CalculateRepairTime(request.EntityId).SingleOrDefaultAsync());
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(RepairRequest), request.EntityId)), null)
            : (Result.Success(), entity.Result);
    }
}

public class CalculateRepairRequestTimeValidator : AbstractValidator<CalculateRepairRequestTimeQuery>
{
    public CalculateRepairRequestTimeValidator()
    {
        // TODO: check all validators of EntityId
        RuleFor(v => v.EntityId).NotEmpty();
    }
}