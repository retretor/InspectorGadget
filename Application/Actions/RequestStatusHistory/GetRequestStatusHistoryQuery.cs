using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.RequestStatusHistory;

public class GetRequestStatusHistoryQuery : IRequest<(Result, Domain.Entities.Basic.RequestStatusHistory?)>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class
    GetAllRequestStatusHistoriesQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.RequestStatusHistory>?)>
{
    public IApplicationDbContext? DbContext { get; set; }
}

public class GetRequestStatusHistoryHandler : IRequestHandler<GetRequestStatusHistoryQuery, (Result,
    Domain.Entities.Basic.RequestStatusHistory?)>
{
    public GetRequestStatusHistoryHandler()
    {
    }

    public async Task<(Result, Domain.Entities.Basic.RequestStatusHistory?)> Handle(
        GetRequestStatusHistoryQuery request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entity = await request.DbContext.RequestStatusHistories.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.RequestStatusHistory), request.Id)),
                null)
            : (Result.Success(), entity);
    }
}

public class GetAllRequestStatusHistoriesHandler : IRequestHandler<GetAllRequestStatusHistoriesQuery,
    (Result, IEnumerable<Domain.Entities.Basic.RequestStatusHistory>?)>
{
    public GetAllRequestStatusHistoriesHandler()
    {
    }

    public async Task<(Result, IEnumerable<Domain.Entities.Basic.RequestStatusHistory>?)> Handle(
        GetAllRequestStatusHistoriesQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entities = await request.DbContext.RequestStatusHistories.ToListAsync(cancellationToken);
        return (Result.Success(), entities);
    }
}

public class GetRequestStatusHistoryValidator : AbstractValidator<GetRequestStatusHistoryQuery>
{
    public GetRequestStatusHistoryValidator()
    {
        RuleFor(v => v.Id).NotEmpty().GreaterThanOrEqualTo(0);
    }
}