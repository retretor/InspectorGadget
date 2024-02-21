using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.PartForRepairType;

public class GetPartForRepairTypeQuery : IRequest<(Result, Domain.Entities.Basic.PartForRepairType?)>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class GetAllPartForRepairTypesQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.PartForRepairType>?)>
{
    public IApplicationDbContext? DbContext { get; set; }
}

public class GetPartForRepairTypeHandler : IRequestHandler<GetPartForRepairTypeQuery, (Result,
    Domain.Entities.Basic.PartForRepairType?)>
{
    public GetPartForRepairTypeHandler()
    {
    }

    public async Task<(Result, Domain.Entities.Basic.PartForRepairType?)> Handle(GetPartForRepairTypeQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entity = await request.DbContext.PartForRepairTypes.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.PartForRepairType), request.Id)), null)
            : (Result.Success(), entity);
    }
}

public class GetAllPartForRepairTypesHandler : IRequestHandler<GetAllPartForRepairTypesQuery, (Result,
    IEnumerable<Domain.Entities.Basic.PartForRepairType>?)>
{
    public GetAllPartForRepairTypesHandler()
    {
    }

    public async Task<(Result, IEnumerable<Domain.Entities.Basic.PartForRepairType>?)> Handle(
        GetAllPartForRepairTypesQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entities = await request.DbContext.PartForRepairTypes.ToListAsync(cancellationToken);
        return (Result.Success(), entities);
    }
}

public class GetPartForRepairTypeValidator : AbstractValidator<GetPartForRepairTypeQuery>
{
    public GetPartForRepairTypeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}