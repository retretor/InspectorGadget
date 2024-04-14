using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairTypeForDevice;

public class DeleteRepairTypeForDeviceCommand : IRequest<Result>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class DeleteRepairTypeForDeviceHandler : IRequestHandler<DeleteRepairTypeForDeviceCommand, Result>
{
    public async Task<Result> Handle(DeleteRepairTypeForDeviceCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }

        var entity = await request.DbContext.RepairTypeForDevices.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(RepairTypeForDevice), request.Id));
        }

        request.DbContext.RepairTypeForDevices.Remove(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class DeleteRepairTypeForDeviceValidator : AbstractValidator<DeleteRepairTypeForDeviceCommand>
{
    public DeleteRepairTypeForDeviceValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}