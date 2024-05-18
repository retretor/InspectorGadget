using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.RepairTypeForDevice;

public class GetRepairTypeForDeviceQuery : IRequest<(Result, Domain.Entities.Basic.RepairTypeForDevice?)>
{
    public int Id { get; init; }
}

public class
    GetAllRepairTypeForDevicesQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.RepairTypeForDevice>?)>
{
}

public class GetRepairTypeForDeviceHandler : BaseHandler, IRequestHandler<GetRepairTypeForDeviceQuery, (Result,
    Domain.Entities.Basic.RepairTypeForDevice?)>
{
    public GetRepairTypeForDeviceHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, Domain.Entities.Basic.RepairTypeForDevice?)> Handle(GetRepairTypeForDeviceQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await DbContext.RepairTypeForDevices.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(RepairTypeForDevice), request.Id)),
                null)
            : (Result.Success(), entity);
    }
}

public class GetAllRepairTypeForDevicesHandler : BaseHandler, IRequestHandler<GetAllRepairTypeForDevicesQuery,
    (Result, IEnumerable<Domain.Entities.Basic.RepairTypeForDevice>?)>
{
    public GetAllRepairTypeForDevicesHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, IEnumerable<Domain.Entities.Basic.RepairTypeForDevice>?)> Handle(
        GetAllRepairTypeForDevicesQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await DbContext.RepairTypeForDevices.ToListAsync(cancellationToken);
        return (Result.Success(), entities);
    }
}

public class GetRepairTypeForDeviceValidator : AbstractValidator<GetRepairTypeForDeviceQuery>
{
    public GetRepairTypeForDeviceValidator()
    {
        RuleFor(v => v.Id).NotEmpty().GreaterThanOrEqualTo(0);
    }
}