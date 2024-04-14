using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Client;

public class GetClientQuery : IRequest<(Result, Domain.Entities.Basic.Client?)>
{
    public int Id { get; init; }
}

public class GetAllClientsQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.Client>?)>
{
}

public class GetClientHandler : BaseHandler, IRequestHandler<GetClientQuery, (Result, Domain.Entities.Basic.Client?)>
{
    public GetClientHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, Domain.Entities.Basic.Client?)> Handle(GetClientQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await DbContext.Clients
            .FirstOrDefaultAsync(client => client.EntityId == request.Id, cancellationToken);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.Client), request.Id)), null)
            : (Result.Success(), entity);
    }
}

public class
    GetAllClientsHandler : BaseHandler,
    IRequestHandler<GetAllClientsQuery, (Result, IEnumerable<Domain.Entities.Basic.Client>?)>
{
    public GetAllClientsHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, IEnumerable<Domain.Entities.Basic.Client>?)> Handle(GetAllClientsQuery request,
        CancellationToken cancellationToken)
    {
        var clients = await DbContext.Clients.ToListAsync(cancellationToken);
        return (Result.Success(), clients);
    }
}

public class GetClientValidator : AbstractValidator<GetClientQuery>
{
    public GetClientValidator()
    {
        RuleFor(v => v.Id).NotEmpty().GreaterThanOrEqualTo(0);
    }
}