using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.DbResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Device;

public class GetRepairTypesInfoForDeviceQuery : IRequest<(Result, List<RepairTypeInfoResult>?)>
{
    public int EntityId { get; init; }
}

public class GetRepairTypesInfoForDeviceHandler : BaseHandler,
    IRequestHandler<GetRepairTypesInfoForDeviceQuery, (Result, List<RepairTypeInfoResult>?)>
{
    public GetRepairTypesInfoForDeviceHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, List<RepairTypeInfoResult>?)> Handle(GetRepairTypesInfoForDeviceQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await Task.Run(() =>
            DbContext.GetRepairTypesInfo(request.EntityId).ToList());
        return entity.Count == 0
            ? (Result.Failure(new NotFoundException(nameof(RepairType), request.EntityId)), null)
            : (Result.Success(), entity);
    }
}

public class GetRepairTypesInfoForDeviceValidator : AbstractValidator<GetRepairTypesInfoForDeviceQuery>
{
    public GetRepairTypesInfoForDeviceValidator()
    {
        // TODO: check all validators of EntityId
        RuleFor(v => v.EntityId).NotEmpty();
    }
}