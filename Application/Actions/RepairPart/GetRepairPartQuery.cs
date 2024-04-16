using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.RepairPart;

public class GetRepairPartQuery : IRequest<(Result, Domain.Entities.Basic.RepairPart?)>
{
    public int Id { get; init; }
}

public class GetAllRepairPartsQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.RepairPart>?)>
{
}

public class GetRepairPartHandler : BaseHandler,
    IRequestHandler<GetRepairPartQuery, (Result, Domain.Entities.Basic.RepairPart?)>
{
    public GetRepairPartHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, Domain.Entities.Basic.RepairPart?)> Handle(GetRepairPartQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await DbContext.RepairParts.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.RepairPart), request.Id)), null)
            : (Result.Success(), entity);
    }
}

public class
    GetAllRepairPartsHandler : BaseHandler,
    IRequestHandler<GetAllRepairPartsQuery, (Result, IEnumerable<Domain.Entities.Basic.RepairPart>?)>
{
    public GetAllRepairPartsHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, IEnumerable<Domain.Entities.Basic.RepairPart>?)> Handle(GetAllRepairPartsQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await DbContext.RepairParts.ToListAsync(cancellationToken);
        return (Result.Success(), entities);
    }
}

public class GetRepairPartValidator : AbstractValidator<GetRepairPartQuery>
{
    public GetRepairPartValidator()
    {
        RuleFor(v => v.Id).NotEmpty().GreaterThanOrEqualTo(0);
    }
}