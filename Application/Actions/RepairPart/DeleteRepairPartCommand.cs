using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairPart;

public class DeleteRepairPartCommand : IRequest<Result>
{
    public int Id { get; init; }
}

public class DeleteRepairPartHandler : BaseHandler, IRequestHandler<DeleteRepairPartCommand, Result>
{
    public DeleteRepairPartHandler(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<Result> Handle(DeleteRepairPartCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.RepairParts.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(RepairPart), request.Id));
        }

        DbContext.RepairParts.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class DeleteRepairPartValidator : AbstractValidator<DeleteRepairPartCommand>
{
    public DeleteRepairPartValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}