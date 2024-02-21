using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairRequest;

public class DeleteRepairRequestCommand : IRequest<Result>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class DeleteRepairRequestHandler : IRequestHandler<DeleteRepairRequestCommand, Result>
{
    public DeleteRepairRequestHandler()
    {
    }

    public async Task<Result> Handle(DeleteRepairRequestCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }

        var entity = await request.DbContext.RepairRequests.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.RepairRequest), request.Id));
        }

        request.DbContext.RepairRequests.Remove(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class DeleteRepairRequestValidator : AbstractValidator<DeleteRepairRequestCommand>
{
    public DeleteRepairRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}