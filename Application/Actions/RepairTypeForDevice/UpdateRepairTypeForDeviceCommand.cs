using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairTypeForDevice;

public class UpdateRepairTypeForDeviceCommand : IRequest<Result>
{
    public int EntityId { get; init; }
    public float Cost { get; init; }
    public int DaysToComplete { get; init; }
    public int RepairTypeId { get; init; }
    public int DeviceId { get; init; }
}

public class UpdateRepairTypeForDeviceHandler : BaseHandler, IRequestHandler<UpdateRepairTypeForDeviceCommand, Result>
{
    public UpdateRepairTypeForDeviceHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<Result> Handle(UpdateRepairTypeForDeviceCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.RepairTypeForDevices.FindAsync(request.EntityId);

        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(RepairTypeForDevice), request.EntityId));
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
        RuleFor(x => x.EntityId).NotEmpty();
        RuleFor(x => x.Cost).NotEmpty().GreaterThan(0);
        RuleFor(x => x.DaysToComplete).NotEmpty().GreaterThan(0);
        RuleFor(x => x.RepairTypeId).NotEmpty();
        RuleFor(x => x.DeviceId).NotEmpty();
    }
}