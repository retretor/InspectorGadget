using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.RepairType;

public class GetRepairTypeQuery : IRequest<(Result, Domain.Entities.Basic.RepairType?)>
{
    public int Id { get; init; }
}

public class GetAllRepairTypesQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.RepairType>?)>
{
}

public class GetRepairTypeHandler : BaseHandler,
    IRequestHandler<GetRepairTypeQuery, (Result, Domain.Entities.Basic.RepairType?)>
{
    public GetRepairTypeHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, Domain.Entities.Basic.RepairType?)> Handle(GetRepairTypeQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await DbContext.RepairTypes.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(RepairType), request.Id)), null)
            : (Result.Success(), entity);
    }
}

public class
    GetAllRepairTypesHandler : BaseHandler, IRequestHandler<GetAllRepairTypesQuery, (Result,
    IEnumerable<Domain.Entities.Basic.RepairType>?)>
{
    public GetAllRepairTypesHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, IEnumerable<Domain.Entities.Basic.RepairType>?)> Handle(GetAllRepairTypesQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await DbContext.RepairTypes.ToListAsync(cancellationToken);
        return (Result.Success(), entities);
    }
}

public class GetRepairTypeValidator : AbstractValidator<GetRepairTypeQuery>
{
    public GetRepairTypeValidator()
    {
        RuleFor(v => v.Id).NotEmpty().GreaterThanOrEqualTo(0);
    }
}