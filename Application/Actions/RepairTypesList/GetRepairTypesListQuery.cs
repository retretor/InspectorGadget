using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.RepairTypesList;

public class GetRepairTypesListQuery : IRequest<(Result, Domain.Entities.Basic.RepairTypesList?)>
{
    public int Id { get; init; }
}

public class GetAllRepairTypesListsQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.RepairTypesList>?)>
{
}

public class GetRepairTypesListHandler : BaseHandler,
    IRequestHandler<GetRepairTypesListQuery, (Result, Domain.Entities.Basic.RepairTypesList?)>
{
    public GetRepairTypesListHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, Domain.Entities.Basic.RepairTypesList?)> Handle(GetRepairTypesListQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await DbContext.RepairTypesLists.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.RepairTypesList), request.Id)), null)
            : (Result.Success(), entity);
    }
}

public class GetAllRepairTypesListsHandler : BaseHandler, IRequestHandler<GetAllRepairTypesListsQuery,
    (Result, IEnumerable<Domain.Entities.Basic.RepairTypesList>?)>
{
    public GetAllRepairTypesListsHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, IEnumerable<Domain.Entities.Basic.RepairTypesList>?)> Handle(
        GetAllRepairTypesListsQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await DbContext.RepairTypesLists.ToListAsync(cancellationToken);
        return (Result.Success(), entities);
    }
}

public class GetRepairTypesListValidator : AbstractValidator<GetRepairTypesListQuery>
{
    public GetRepairTypesListValidator()
    {
        RuleFor(v => v.Id).NotEmpty().GreaterThanOrEqualTo(0);
    }
}