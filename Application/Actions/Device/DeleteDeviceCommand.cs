using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.Device;

public class DeleteDeviceCommand : IRequest<Result>
{
    public int Id { get; init; }
}

public class DeleteDeviceHandler : BaseHandler, IRequestHandler<DeleteDeviceCommand, Result>
{
    public DeleteDeviceHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.Devices.FindAsync(request.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Basic.Device), request.Id);
        }

        DbContext.Devices.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class DeleteDeviceValidator : AbstractValidator<DeleteDeviceCommand>
{
    public DeleteDeviceValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}