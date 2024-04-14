using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.RepairRequest;

public class GetRepairRequestQuery : IRequest<(Result, Domain.Entities.Basic.RepairRequest?)>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class GetAllRepairRequestsQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.RepairRequest>?)>
{
    public IApplicationDbContext? DbContext { get; set; }
}

public class
    GetRepairRequestHandler : IRequestHandler<GetRepairRequestQuery, (Result, Domain.Entities.Basic.RepairRequest?)>
{
    // TODO: delete constructor if not needed

    public async Task<(Result, Domain.Entities.Basic.RepairRequest?)> Handle(GetRepairRequestQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entity = await request.DbContext.RepairRequests.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.RepairRequest), request.Id)), null)
            : (Result.Success(), entity);
    }
}

public class GetAllRepairRequestsHandler : IRequestHandler<GetAllRepairRequestsQuery,
    (Result, IEnumerable<Domain.Entities.Basic.RepairRequest>?)>
{
    public async Task<(Result, IEnumerable<Domain.Entities.Basic.RepairRequest>?)> Handle(
        GetAllRepairRequestsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entities = await request.DbContext.RepairRequests.ToListAsync(cancellationToken);
        return (Result.Success(), entities);
    }
}

public class GetRepairRequestValidator : AbstractValidator<GetRepairRequestQuery>
{
    public GetRepairRequestValidator()
    {
        RuleFor(v => v.Id).NotEmpty().GreaterThanOrEqualTo(0);
    }
}