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
    public IApplicationDbContext? DbContext { get; set; }
}

public class CreateRepairTypeForDeviceHandler : IRequestHandler<CreateRepairTypeForDeviceCommand, (Result, int?)>
{
    private readonly IMapper _mapper;

    public CreateRepairTypeForDeviceHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<(Result, int?)> Handle(CreateRepairTypeForDeviceCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }
        var entity = _mapper.Map<Domain.Entities.Basic.RepairTypeForDevice>(request);
        request.DbContext.RepairTypeForDevices.Add(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);
        return (Result.Success(), entity.Id);
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