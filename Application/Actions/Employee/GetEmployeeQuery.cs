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
}

public class GetAllEmployeesQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.Employee>?)>
{
}

public class GetEmployeeHandler : BaseHandler,
    IRequestHandler<GetEmployeeQuery, (Result, Domain.Entities.Basic.Employee?)>
{
    public GetEmployeeHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, Domain.Entities.Basic.Employee?)> Handle(GetEmployeeQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await DbContext.Employees.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.Employee), request.Id)), null)
            : (Result.Success(), entity);
    }
}

public class GetAllEmployeesHandler : BaseHandler,
    IRequestHandler<GetAllEmployeesQuery, (Result, IEnumerable<Domain.Entities.Basic.Employee>?)>
{
    public GetAllEmployeesHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, IEnumerable<Domain.Entities.Basic.Employee>?)> Handle(GetAllEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await DbContext.Employees.ToListAsync(cancellationToken);
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