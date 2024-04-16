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
}

public class GetAllDbUsersQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.DbUser>?)>
{
}

public class GetDbUserHandler : BaseHandler, IRequestHandler<GetDbUserQuery, (Result, Domain.Entities.Basic.DbUser?)>
{
    public GetDbUserHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, Domain.Entities.Basic.DbUser?)> Handle(GetDbUserQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await DbContext.DbUsers.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.DbUser), request.Id)), null)
            : (Result.Success(), entity);
    }
}

public class
    GetAllDbUsersHandler : BaseHandler,
    IRequestHandler<GetAllDbUsersQuery, (Result, IEnumerable<Domain.Entities.Basic.DbUser>?)>
{
    public GetAllDbUsersHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, IEnumerable<Domain.Entities.Basic.DbUser>?)> Handle(GetAllDbUsersQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await DbContext.DbUsers.ToListAsync(cancellationToken);
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