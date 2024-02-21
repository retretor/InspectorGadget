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
    public IApplicationDbContext? DbContext { get; set; }
}

public class
    GetAllRepairTypeForDevicesQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.RepairTypeForDevice>?)>
{
    public IApplicationDbContext? DbContext { get; set; }
}

public class GetRepairTypeForDeviceHandler : IRequestHandler<GetRepairTypeForDeviceQuery, (Result,
    Domain.Entities.Basic.RepairTypeForDevice?)>
{
    public GetRepairTypeForDeviceHandler()
    {
    }

    public async Task<(Result, Domain.Entities.Basic.RepairTypeForDevice?)> Handle(GetRepairTypeForDeviceQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entity = await request.DbContext.RepairTypeForDevices.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.RepairTypeForDevice), request.Id)),
                null)
            : (Result.Success(), entity);
    }
}

public class GetAllRepairTypeForDevicesHandler : IRequestHandler<GetAllRepairTypeForDevicesQuery,
    (Result, IEnumerable<Domain.Entities.Basic.RepairTypeForDevice>?)>
{
    public GetAllRepairTypeForDevicesHandler()
    {
    }

    public async Task<(Result, IEnumerable<Domain.Entities.Basic.RepairTypeForDevice>?)> Handle(
        GetAllRepairTypeForDevicesQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }
        var entities = await request.DbContext.RepairTypeForDevices.ToListAsync(cancellationToken);
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