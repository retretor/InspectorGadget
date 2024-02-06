using Application.Common.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairTypeForDevice;

public class GetRepairTypeForDeviceQuery : IRequest<Domain.Entities.Basic.RepairTypeForDevice>
{
    public int Id { get; init; }
    public float Cost { get; init; }
    public int Time { get; init; }
    public int RepairTypeId { get; init; }
    public int DeviceId { get; init; }
}

public class GetRepairTypeForDeviceQueryHandler : IRequestHandler<GetRepairTypeForDeviceQuery,
    Domain.Entities.Basic.RepairTypeForDevice>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRepairTypeForDeviceQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Domain.Entities.Basic.RepairTypeForDevice> Handle(GetRepairTypeForDeviceQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.RepairTypeForDevices.FindAsync(request.Id);
        return _mapper.Map<Domain.Entities.Basic.RepairTypeForDevice>(entity);
    }
}

public class GetRepairTypeForDeviceQueryValidator : AbstractValidator<GetRepairTypeForDeviceQuery>
{
    public GetRepairTypeForDeviceQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Cost).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Time).NotEmpty().GreaterThan(0);
        RuleFor(x => x.RepairTypeId).NotEmpty();
        RuleFor(x => x.DeviceId).NotEmpty();
    }
}