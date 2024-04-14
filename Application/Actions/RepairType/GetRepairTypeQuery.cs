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
    public IApplicationDbContext? DbContext { get; set; }
}

public class GetAllRepairTypesQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.RepairType>?)>
{
    public IApplicationDbContext? DbContext { get; set; }
}

public class GetRepairTypeHandler : IRequestHandler<GetRepairTypeQuery, (Result, Domain.Entities.Basic.RepairType?)>
{
    public async Task<(Result, Domain.Entities.Basic.RepairType?)> Handle(GetRepairTypeQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entity = await request.DbContext.RepairTypes.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.RepairType), request.Id)), null)
            : (Result.Success(), entity);
    }
}

public class
    GetAllRepairTypesHandler : IRequestHandler<GetAllRepairTypesQuery, (Result,
    IEnumerable<Domain.Entities.Basic.RepairType>?)>
{
    public async Task<(Result, IEnumerable<Domain.Entities.Basic.RepairType>?)> Handle(GetAllRepairTypesQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entities = await request.DbContext.RepairTypes.ToListAsync(cancellationToken);
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