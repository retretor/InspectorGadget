using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.Device;

public class GetDeviceQuery : IRequest<(Result, Domain.Entities.Basic.Device?)>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class GetAllDevicesQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.Device>?)>
{
    public IApplicationDbContext? DbContext { get; set; }
}

public class GetDeviceHandler : IRequestHandler<GetDeviceQuery, (Result, Domain.Entities.Basic.Device?)>
{
    public GetDeviceHandler()
    {
    }

    public async Task<(Result, Domain.Entities.Basic.Device?)> Handle(GetDeviceQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entity = await request.DbContext.Devices.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.Device), request.Id)), null)
            : (Result.Success(), entity);
    }
}

public class
    GetAllDevicesHandler : IRequestHandler<GetAllDevicesQuery, (Result, IEnumerable<Domain.Entities.Basic.Device>?)>
{
    public GetAllDevicesHandler()
    {
    }

    public async Task<(Result, IEnumerable<Domain.Entities.Basic.Device>?)> Handle(GetAllDevicesQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entities = await request.DbContext.Devices.ToListAsync(cancellationToken);
        return (Result.Success(), entities);
    }
}

public class GetDeviceValidator : AbstractValidator<GetDeviceQuery>
{
    public GetDeviceValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}