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
}

public class GetAllDevicesQuery : IRequest<(Result, IEnumerable<Domain.Entities.Basic.Device>?)>
{
}

public class GetDeviceHandler : BaseHandler, IRequestHandler<GetDeviceQuery, (Result, Domain.Entities.Basic.Device?)>
{
    public GetDeviceHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, Domain.Entities.Basic.Device?)> Handle(GetDeviceQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await DbContext.Devices.FindAsync(request.Id);
        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(Device), request.Id)), null)
            : (Result.Success(), entity);
    }
}

public class
    GetAllDevicesHandler : BaseHandler,
    IRequestHandler<GetAllDevicesQuery, (Result, IEnumerable<Domain.Entities.Basic.Device>?)>
{
    public GetAllDevicesHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, IEnumerable<Domain.Entities.Basic.Device>?)> Handle(GetAllDevicesQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await DbContext.Devices.ToListAsync(cancellationToken);
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