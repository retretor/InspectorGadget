using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Actions.Device;

public class UpdateDeviceCommand : IRequest<Result>
{
    public int EntityId { get; init; }
    public string Name { get; init; } = null!;
    public string Type { get; init; } = null!;
    public string Brand { get; init; } = null!;
    public string Series { get; init; } = null!;
    public string Manufacturer { get; init; } = null!;
}

public class UpdateDeviceHandler : BaseHandler, IRequestHandler<UpdateDeviceCommand, Result>
{
    public UpdateDeviceHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<Result> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.Devices.FindAsync(request.EntityId);

        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.Device), request.EntityId));
        }

        Mapper!.Map(request, entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class UpdateDeviceValidator : AbstractValidator<UpdateDeviceCommand>
{
    public UpdateDeviceValidator()
    {
        RuleFor(x => x.EntityId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Type).NotEmpty().IsEnumName(typeof(DeviceType));
        RuleFor(x => x.Brand).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Series).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Manufacturer).NotEmpty().MaximumLength(255);
    }
}