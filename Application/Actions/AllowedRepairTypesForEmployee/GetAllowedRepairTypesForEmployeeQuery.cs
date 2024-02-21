using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.AllowedRepairTypesForEmployee;

public class
    GetAllowedRepairTypesForEmployeeQuery : IRequest<(Result, Domain.Entities.Basic.AllowedRepairTypesForEmployee?)>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class
    GetAllAllowedRepairTypesForEmployeeQuery : IRequest<(Result,
    IEnumerable<Domain.Entities.Basic.AllowedRepairTypesForEmployee>?)>
{
    public IApplicationDbContext? DbContext { get; set; }
}

public class GetAllowedRepairTypesForEmployeeHandler : IRequestHandler<GetAllowedRepairTypesForEmployeeQuery, (Result,
    Domain.Entities.Basic.AllowedRepairTypesForEmployee?)>
{
    public GetAllowedRepairTypesForEmployeeHandler()
    {
    }

    public async Task<(Result, Domain.Entities.Basic.AllowedRepairTypesForEmployee?)> Handle(
        GetAllowedRepairTypesForEmployeeQuery request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entity = await request.DbContext.AllowedRepairTypesForEmployees.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(AllowedRepairTypesForEmployee), request.Id)), null)
            : (Result.Success(), entity);
    }
}

public class GetAllAllowedRepairTypesForEmployeeHandler : IRequestHandler<GetAllAllowedRepairTypesForEmployeeQuery,
    (Result, IEnumerable<Domain.Entities.Basic.AllowedRepairTypesForEmployee>?)>
{
    public GetAllAllowedRepairTypesForEmployeeHandler()
    {
    }

    public async Task<(Result, IEnumerable<Domain.Entities.Basic.AllowedRepairTypesForEmployee>?)> Handle(
        GetAllAllowedRepairTypesForEmployeeQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entities = await request.DbContext.AllowedRepairTypesForEmployees.ToListAsync(cancellationToken);
        return (Result.Success(), entities);
    }
}

public class GetAllowedRepairTypesForEmployeeValidator : AbstractValidator<GetAllowedRepairTypesForEmployeeQuery>
{
    public GetAllowedRepairTypesForEmployeeValidator()
    {
        RuleFor(v => v.Id).NotEmpty().GreaterThanOrEqualTo(0);
    }
}