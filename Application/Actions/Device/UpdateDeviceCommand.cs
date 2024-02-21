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
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string Type { get; init; } = null!;
    public string Brand { get; init; } = null!;
    public string Series { get; init; } = null!;
    public string Manufacturer { get; init; } = null!;
    public IApplicationDbContext? DbContext { get; set; }
}

public class UpdateDeviceHandler : IRequestHandler<UpdateDeviceCommand, Result>
{
    private readonly IMapper _mapper;

    public UpdateDeviceHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }

        var entity = await request.DbContext.Devices.FindAsync(request.Id);

        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.Device), request.Id));
        }

        _mapper.Map(request, entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class UpdateDeviceValidator : AbstractValidator<UpdateDeviceCommand>
{
    public UpdateDeviceValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Type).NotEmpty().IsEnumName(typeof(DeviceType));
        RuleFor(x => x.Brand).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Series).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Manufacturer).NotEmpty().MaximumLength(255);
    }
}