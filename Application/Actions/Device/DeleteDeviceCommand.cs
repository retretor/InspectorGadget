using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.Device;

public class DeleteDeviceCommand : IRequest<Result>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class DeleteDeviceHandler : IRequestHandler<DeleteDeviceCommand, Result>
{
    public DeleteDeviceHandler()
    {
    }

    public async Task<Result> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }

        var entity = await request.DbContext.Devices.FindAsync(request.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Basic.Device), request.Id);
        }

        request.DbContext.Devices.Remove(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);

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