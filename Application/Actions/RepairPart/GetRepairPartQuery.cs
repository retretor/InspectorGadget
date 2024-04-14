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
    public IApplicationDbContext? DbContext { get; set; }
}

public class GetAllRepairPartsQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.RepairPart>?)>
{
    public IApplicationDbContext? DbContext { get; set; }
}

public class GetRepairPartHandler : IRequestHandler<GetRepairPartQuery, (Result, Domain.Entities.Basic.RepairPart?)>
{
    public async Task<(Result, Domain.Entities.Basic.RepairPart?)> Handle(GetRepairPartQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entity = await request.DbContext.RepairParts.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.RepairPart), request.Id)), null)
            : (Result.Success(), entity);
    }
}

public class
    GetAllRepairPartsHandler : IRequestHandler<GetAllRepairPartsQuery, (Result, IEnumerable<Domain.Entities.Basic.RepairPart>?)>
{
    public async Task<(Result, IEnumerable<Domain.Entities.Basic.RepairPart>?)> Handle(GetAllRepairPartsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }
        var entities = await request.DbContext.RepairParts.ToListAsync(cancellationToken);
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