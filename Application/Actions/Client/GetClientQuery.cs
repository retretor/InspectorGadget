using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Client;

public class GetClientQuery : IRequest<(Result, GetClientResponse?)>
{
    public int DbUserId { get; init; }
}

public class GetAllClientsQuery : IRequest<(Result, GetAllClientsResponse?)>
{
}

public class GetClientHandler : BaseHandler, IRequestHandler<GetClientQuery, (Result, GetClientResponse?)>
{
    public GetClientHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<(Result, GetClientResponse?)> Handle(GetClientQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await DbContext.Clients
            .FirstOrDefaultAsync(client => client.DbUserId == request.DbUserId, cancellationToken);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(DbUser), request.DbUserId)), null)
            : (Result.Success(), Mapper!.Map<GetClientResponse>(entity));
    }
}

public class
    GetAllClientsHandler : BaseHandler,
    IRequestHandler<GetAllClientsQuery, (Result, GetAllClientsResponse?)>
{
    public GetAllClientsHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<(Result, GetAllClientsResponse?)> Handle(GetAllClientsQuery request,
        CancellationToken cancellationToken)
    {
        var clients = await DbContext.Clients.ToListAsync(cancellationToken);
        return (Result.Success(),
            new GetAllClientsResponse { Clients = Mapper!.Map<List<GetClientResponse>>(clients) });
    }
}

public class GetClientValidator : AbstractValidator<GetClientQuery>
{
    public GetClientValidator()
    {
        RuleFor(v => v.DbUserId).NotEmpty().GreaterThanOrEqualTo(0);
    }
}