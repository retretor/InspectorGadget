using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.DbUser;

public class GetDbUserQuery : IRequest<(Result, GetDbUserResponse?)>
{
    public int Id { get; init; }
}

public class GetAllDbUsersQuery : IRequest<(Result, GetAllDbUsersResponse?)>
{
}

public class GetDbUserHandler : BaseHandler, IRequestHandler<GetDbUserQuery, (Result, GetDbUserResponse?)>
{
    public GetDbUserHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<(Result, GetDbUserResponse?)> Handle(GetDbUserQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await DbContext.DbUsers.FindAsync(request.Id);
        Domain.Entities.Basic.Employee? employee = null;
        Domain.Entities.Basic.Client? client = null;
        try
        {
            employee = await DbContext.Employees.FirstOrDefaultAsync(e => e.DbUserId == request.Id, cancellationToken);
            Console.WriteLine("Employee found");
        }
        catch
        {
            Console.WriteLine("Trying to get employee failed");
        }

        try
        {
            client = await DbContext.Clients.FirstOrDefaultAsync(c => c.DbUserId == request.Id, cancellationToken);
            Console.WriteLine("Client found");
        }
        catch
        {
            Console.WriteLine("Trying to get client failed");
        }

        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(DbUser), request.Id)), null)
            : (Result.Success(), Mapper!.Map<GetDbUserResponse>((entity, employee, client)));
    }
}

public class
    GetAllDbUsersHandler : BaseHandler,
    IRequestHandler<GetAllDbUsersQuery, (Result, GetAllDbUsersResponse?)>
{
    public GetAllDbUsersHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<(Result, GetAllDbUsersResponse?)> Handle(GetAllDbUsersQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await DbContext.DbUsers.ToListAsync(cancellationToken);
        var response = new List<GetDbUserResponse>();
        foreach (var entity in entities)
        {
            var employee =
                await DbContext.Employees.FirstOrDefaultAsync(e => e.DbUserId == entity.EntityId, cancellationToken);
            var client =
                await DbContext.Clients.FirstOrDefaultAsync(c => c.DbUserId == entity.EntityId, cancellationToken);
            response.Add(Mapper!.Map<GetDbUserResponse>((entity, employee, client)));
        }

        return (Result.Success(), new GetAllDbUsersResponse { DbUsers = response });
    }
}

public class GetDbUserValidator : AbstractValidator<GetDbUserQuery>
{
    public GetDbUserValidator()
    {
        RuleFor(v => v.Id).NotEmpty().GreaterThanOrEqualTo(0);
    }
}