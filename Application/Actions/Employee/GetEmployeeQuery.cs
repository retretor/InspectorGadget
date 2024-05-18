using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Employee;

public class GetEmployeeQuery : IRequest<(Result, GetEmployeeResponse?)>
{
    public int DbUserId { get; init; }
}

public class GetAllEmployeesQuery : IRequest<(Result, GetAllEmployeesResponse?)>
{
}

public class GetEmployeeHandler : BaseHandler,
    IRequestHandler<GetEmployeeQuery, (Result, GetEmployeeResponse?)>
{
    public GetEmployeeHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<(Result, GetEmployeeResponse?)> Handle(GetEmployeeQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await DbContext.Employees
            .FirstOrDefaultAsync(employee => employee.DbUserId == request.DbUserId, cancellationToken);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(DbUser), request.DbUserId)), null)
            : (Result.Success(), Mapper!.Map<GetEmployeeResponse>(entity));
    }
}

public class GetAllEmployeesHandler : BaseHandler,
    IRequestHandler<GetAllEmployeesQuery, (Result, GetAllEmployeesResponse?)>
{
    public GetAllEmployeesHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<(Result, GetAllEmployeesResponse?)> Handle(GetAllEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await DbContext.Employees.ToListAsync(cancellationToken);
        return (Result.Success(),
            new GetAllEmployeesResponse { Employees = Mapper!.Map<List<GetEmployeeResponse>>(entities) });
    }
}

public class GetEmployeeValidator : AbstractValidator<GetEmployeeQuery>
{
    public GetEmployeeValidator()
    {
        RuleFor(v => v.DbUserId).NotEmpty().GreaterThanOrEqualTo(0);
    }
}