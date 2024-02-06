using Application.Common.Interfaces;
using AutoMapper;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Actions.Device;

public class GetDeviceQuery : IRequest<Domain.Entities.Basic.Device>
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public DeviceType Type { get; init; }
    public string Brand { get; init; } = string.Empty;
    public string Series { get; init; } = string.Empty;
    public string Manufacturer { get; init; } = string.Empty;
}

public class GetDeviceQueryHandler : IRequestHandler<GetDeviceQuery, Domain.Entities.Basic.Device>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDeviceQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Domain.Entities.Basic.Device> Handle(GetDeviceQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Devices.FindAsync(request.Id);
        return _mapper.Map<Domain.Entities.Basic.Device>(entity);
    }
}

public class GetDeviceQueryValidator : AbstractValidator<GetDeviceQuery>
{
    public GetDeviceQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Type).NotEmpty();
        RuleFor(x => x.Brand).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Series).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Manufacturer).NotEmpty().MaximumLength(255);
    }
}