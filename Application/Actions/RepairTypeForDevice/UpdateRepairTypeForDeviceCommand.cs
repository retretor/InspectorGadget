using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.RepairTypeForDevice;

public class UpdateRepairTypeForDeviceCommand : IRequest<Result>
{
    public int DeviceId { get; init; }
    public int RepairTypeId { get; init; }
    public float Cost { get; init; }
    public int DaysToComplete { get; init; }
}

public class UpdateRepairTypeForDeviceHandler : BaseHandler, IRequestHandler<UpdateRepairTypeForDeviceCommand, Result>
{
    public UpdateRepairTypeForDeviceHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<Result> Handle(UpdateRepairTypeForDeviceCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.RepairTypeForDevices.FirstOrDefaultAsync(x =>
            x.DeviceId == request.DeviceId && x.RepairTypeId == request.RepairTypeId, cancellationToken);

        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(RepairTypeForDevice),
                $"{request.DeviceId} {request.RepairTypeId}"));
        }

        Mapper!.Map(request, entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class UpdateRepairTypeForDeviceValidator : AbstractValidator<UpdateRepairTypeForDeviceCommand>
{
    public UpdateRepairTypeForDeviceValidator()
    {
        RuleFor(x => x.Cost).NotEmpty().GreaterThan(0);
        RuleFor(x => x.DaysToComplete).NotEmpty().GreaterThan(0);
        RuleFor(x => x.RepairTypeId).NotEmpty();
        RuleFor(x => x.DeviceId).NotEmpty();
    }
}