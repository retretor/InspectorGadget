using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairTypeForDevice;

public class CreateRepairTypeForDeviceCommand : IRequest<(Result, int?)>
{
    public float Cost { get; init; }
    public int DaysToComplete { get; init; }
    public int RepairTypeId { get; init; }
    public int DeviceId { get; init; }
}

public class CreateRepairTypeForDeviceHandler : BaseHandler,
    IRequestHandler<CreateRepairTypeForDeviceCommand, (Result, int?)>
{
    public CreateRepairTypeForDeviceHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<(Result, int?)> Handle(CreateRepairTypeForDeviceCommand request,
        CancellationToken cancellationToken)
    {
        var entity = Mapper!.Map<Domain.Entities.Basic.RepairTypeForDevice>(request);
        DbContext.RepairTypeForDevices.Add(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
        return (Result.Success(), entity.EntityId);
    }
}

public class CreateRepairTypeForDeviceValidator : AbstractValidator<CreateRepairTypeForDeviceCommand>
{
    public CreateRepairTypeForDeviceValidator()
    {
        RuleFor(x => x.Cost).NotEmpty().GreaterThan(0);
        RuleFor(x => x.DaysToComplete).NotEmpty().GreaterThan(0);
        RuleFor(x => x.RepairTypeId).NotEmpty();
        RuleFor(x => x.DeviceId).NotEmpty();
    }
}