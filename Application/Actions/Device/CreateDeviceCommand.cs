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
}

public class CreateDeviceHandler : BaseHandler, IRequestHandler<CreateDeviceCommand, (Result, int?)>
{
    public CreateDeviceHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<(Result, int?)> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        var entity = Mapper!.Map<Domain.Entities.Basic.Device>(request);
        entity.PhotoPath = "default.jpg";
        DbContext.Devices.Add(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
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