using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Actions.Device;

public class CreateDeviceCommand : IRequest<(Result, int?)>
{
    public string Name { get; init; } = null!;
    public string Type { get; init; } = null!;
    public string Brand { get; init; } = null!;
    public string Series { get; init; } = null!;
    public string Manufacturer { get; init; } = null!;
    public IApplicationDbContext? DbContext { get; set; }
}

public class CreateDeviceHandler : IRequestHandler<CreateDeviceCommand, (Result, int?)>
{
    private readonly IMapper _mapper;

    public CreateDeviceHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<(Result, int?)> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Basic.Device>(request);
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        request.DbContext.Devices.Add(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);
        return (Result.Success(), entity.EntityId);
    }
}

public class CreateDeviceValidator : AbstractValidator<CreateDeviceCommand>
{
    public CreateDeviceValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Type).NotEmpty().IsEnumName(typeof(DeviceType));
        RuleFor(x => x.Brand).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Series).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Manufacturer).NotEmpty().MaximumLength(255);
    }
}