using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Employee;

public class GetEmployeeQuery : IRequest<(Result, Domain.Entities.Basic.Employee?)>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class GetAllEmployeesQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.Employee>?)>
{
    public IApplicationDbContext? DbContext { get; set; }
}

public class GetEmployeeHandler : IRequestHandler<GetEmployeeQuery, (Result, Domain.Entities.Basic.Employee?)>
{
    public async Task<(Result, Domain.Entities.Basic.Employee?)> Handle(GetEmployeeQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entity = await request.DbContext.Employees.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.Employee), request.Id)), null)
            : (Result.Success(), entity);
    }
}

public class
    GetAllEmployeesHandler : IRequestHandler<GetAllEmployeesQuery, (Result, IEnumerable<Domain.Entities.Basic.Employee>?
    )>
{
    public async Task<(Result, IEnumerable<Domain.Entities.Basic.Employee>?)> Handle(GetAllEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entities = await request.DbContext.Employees.ToListAsync(cancellationToken);
        return (Result.Success(), entities);
    }
}

public class GetEmployeeValidator : AbstractValidator<GetEmployeeQuery>
{
    public GetEmployeeValidator()
    {
        RuleFor(v => v.Id).NotEmpty().GreaterThanOrEqualTo(0);
    }
}