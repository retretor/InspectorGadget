using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.DbUser;

public class GetDbUserQuery : IRequest<(Result, Domain.Entities.Basic.DbUser?)>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class GetAllDbUsersQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.DbUser>?)>
{
    public IApplicationDbContext? DbContext { get; set; }
}

public class GetDbUserHandler : IRequestHandler<GetDbUserQuery, (Result, Domain.Entities.Basic.DbUser?)>
{
    public async Task<(Result, Domain.Entities.Basic.DbUser?)> Handle(GetDbUserQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entity = await request.DbContext.DbUsers.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.DbUser), request.Id)), null)
            : (Result.Success(), entity);
    }
}

public class
    GetAllDbUsersHandler : IRequestHandler<GetAllDbUsersQuery, (Result, IEnumerable<Domain.Entities.Basic.DbUser>?)>
{
    public async Task<(Result, IEnumerable<Domain.Entities.Basic.DbUser>?)> Handle(GetAllDbUsersQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entities = await request.DbContext.DbUsers.ToListAsync(cancellationToken);
        return (Result.Success(), entities);
    }
}

public class GetDbUserValidator : AbstractValidator<GetDbUserQuery>
{
    public GetDbUserValidator()
    {
        RuleFor(v => v.Id).NotEmpty().GreaterThanOrEqualTo(0);
    }
}