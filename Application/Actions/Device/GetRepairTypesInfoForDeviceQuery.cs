using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.Composite;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Device;

public class GetRepairTypesInfoForDeviceQuery : IRequest<(Result, RepairTypeInfoResult?)>
{
    public int EntityId { get; init; }
}

public class GetRepairTypesInfoForDeviceHandler : BaseHandler,
    IRequestHandler<GetRepairTypesInfoForDeviceQuery, (Result, RepairTypeInfoResult?)>
{
    public GetRepairTypesInfoForDeviceHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, RepairTypeInfoResult?)> Handle(GetRepairTypesInfoForDeviceQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await Task.Run(() =>
            DbContext.GetRepairTypesInfo(request.EntityId).SingleOrDefaultAsync());
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(RepairRequest), request.EntityId)), null)
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