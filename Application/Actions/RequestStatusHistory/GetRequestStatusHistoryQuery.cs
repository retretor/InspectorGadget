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
}

public class
    GetAllRequestStatusHistoriesQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.RequestStatusHistory>?)>
{
}

public class GetRequestStatusHistoryHandler : BaseHandler, IRequestHandler<GetRequestStatusHistoryQuery, (Result,
    Domain.Entities.Basic.RequestStatusHistory?)>
{
    public GetRequestStatusHistoryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, Domain.Entities.Basic.RequestStatusHistory?)> Handle(
        GetRequestStatusHistoryQuery request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.RequestStatusHistories.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(RequestStatusHistory), request.Id)),
                null)
            : (Result.Success(), entity);
    }
}

public class GetAllRequestStatusHistoriesHandler : BaseHandler, IRequestHandler<GetAllRequestStatusHistoriesQuery,
    (Result, IEnumerable<Domain.Entities.Basic.RequestStatusHistory>?)>
{
    public GetAllRequestStatusHistoriesHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, IEnumerable<Domain.Entities.Basic.RequestStatusHistory>?)> Handle(
        GetAllRequestStatusHistoriesQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await DbContext.RequestStatusHistories.ToListAsync(cancellationToken);
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