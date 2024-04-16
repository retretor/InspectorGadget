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
}

public class GetAllRepairRequestsQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.RepairRequest>?)>
{
}

public class
    GetRepairRequestHandler : BaseHandler,
    IRequestHandler<GetRepairRequestQuery, (Result, Domain.Entities.Basic.RepairRequest?)>
{
    // TODO: delete constructor if not needed

    public GetRepairRequestHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, Domain.Entities.Basic.RepairRequest?)> Handle(GetRepairRequestQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await DbContext.RepairRequests.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.RepairRequest), request.Id)), null)
            : (Result.Success(), entity);
    }
}

public class GetAllRepairRequestsHandler : BaseHandler, IRequestHandler<GetAllRepairRequestsQuery,
    (Result, IEnumerable<Domain.Entities.Basic.RepairRequest>?)>
{
    public GetAllRepairRequestsHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, IEnumerable<Domain.Entities.Basic.RepairRequest>?)> Handle(
        GetAllRepairRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await DbContext.RepairRequests.ToListAsync(cancellationToken);
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