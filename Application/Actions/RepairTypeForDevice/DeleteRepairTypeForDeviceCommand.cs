using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairTypeForDevice;

public class DeleteRepairTypeForDeviceCommand : IRequest<Result>
{
    public int Id { get; init; }
}

public class DeleteRepairTypeForDeviceHandler : BaseHandler, IRequestHandler<DeleteRepairTypeForDeviceCommand, Result>
{
    public DeleteRepairTypeForDeviceHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result> Handle(DeleteRepairTypeForDeviceCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.RepairTypeForDevices.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(RepairTypeForDevice), request.Id));
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
        RuleFor(x => x.Id).NotEmpty();
    }
}