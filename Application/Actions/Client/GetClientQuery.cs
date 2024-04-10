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
    public IApplicationDbContext? DbContext { get; set; }
}

public class GetAllClientsQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.Client>?)>
{
    public IApplicationDbContext? DbContext { get; set; }
}

public class GetClientHandler : IRequestHandler<GetClientQuery, (Result, Domain.Entities.Basic.Client?)>
{
    public GetClientHandler()
    {
    }

    public async Task<(Result, Domain.Entities.Basic.Client?)> Handle(GetClientQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entity = await request.DbContext.Clients
            .FirstOrDefaultAsync(client => client.EntityId == request.Id, cancellationToken);
        if (entity == null)
        {
            return (Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.Client), request.Id)), null);
        }

        return (Result.Success(), entity);
    }
}

public class
    GetAllClientsHandler : IRequestHandler<GetAllClientsQuery, (Result, IEnumerable<Domain.Entities.Basic.Client>?)>
{
    public GetAllClientsHandler()
    {
    }

    public async Task<(Result, IEnumerable<Domain.Entities.Basic.Client>?)> Handle(GetAllClientsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var clients = await request.DbContext.Clients.ToListAsync(cancellationToken);
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