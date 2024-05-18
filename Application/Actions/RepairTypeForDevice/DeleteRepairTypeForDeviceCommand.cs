using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.RepairTypeForDevice;

public class DeleteRepairTypeForDeviceCommand : IRequest<Result>
{
    public int DeviceId { get; init; }
    public int RepairTypeId { get; init; }
}

public class DeleteRepairTypeForDeviceHandler : BaseHandler, IRequestHandler<DeleteRepairTypeForDeviceCommand, Result>
{
    public DeleteRepairTypeForDeviceHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result> Handle(DeleteRepairTypeForDeviceCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.RepairTypeForDevices.FirstOrDefaultAsync(x =>
            x.DeviceId == request.DeviceId && x.RepairTypeId == request.RepairTypeId, cancellationToken);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(RepairTypeForDevice), $"{request.DeviceId} {request.RepairTypeId}"));
        }

        DbContext.RepairTypeForDevices.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class DeleteRepairTypeForDeviceValidator : AbstractValidator<DeleteRepairTypeForDeviceCommand>
{
    public DeleteRepairTypeForDeviceValidator()
    {
        RuleFor(x => x.RepairTypeId).NotEmpty();
        RuleFor(x => x.DeviceId).NotEmpty();
    }
}