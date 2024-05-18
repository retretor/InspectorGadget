using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairRequest;

public class DeleteRepairRequestCommand : IRequest<Result>
{
    public int Id { get; init; }
}

public class DeleteRepairRequestHandler : BaseHandler, IRequestHandler<DeleteRepairRequestCommand, Result>
{
    public DeleteRepairRequestHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result> Handle(DeleteRepairRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.RepairRequests.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(RepairRequest), request.Id));
        }

        DbContext.RepairRequests.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);

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