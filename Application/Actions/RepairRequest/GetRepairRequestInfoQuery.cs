using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.Composite;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.RepairRequest;

public class GetRepairRequestInfoQuery : IRequest<(Result, RepairRequestInfoResult?)>
{
    public int EntityId { get; init; }
}

public class
    GetRepairRequestInfoHandler : BaseHandler,
    IRequestHandler<GetRepairRequestInfoQuery, (Result, RepairRequestInfoResult?)>
{
    public GetRepairRequestInfoHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, RepairRequestInfoResult?)> Handle(GetRepairRequestInfoQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await Task.Run(() =>
            DbContext.GetRequestInfo(request.EntityId).SingleOrDefaultAsync());
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(RepairRequest), request.EntityId)), null)
            : (Result.Success(), entity);
    }
}

public class GetRepairRequestInfoValidator : AbstractValidator<GetRepairRequestInfoQuery>
{
    public GetRepairRequestInfoValidator()
    {
        // TODO: check all validators of EntityId
        RuleFor(v => v.EntityId).NotEmpty();
    }
}