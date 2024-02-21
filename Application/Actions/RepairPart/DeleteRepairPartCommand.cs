using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairPart;

public class DeleteRepairPartCommand : IRequest<Result>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class DeleteRepairPartHandler : IRequestHandler<DeleteRepairPartCommand, Result>
{
    public DeleteRepairPartHandler(IApplicationDbContext context)
    {
    }

    public async Task<Result> Handle(DeleteRepairPartCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }

        var entity = await request.DbContext.RepairParts.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.RepairPart), request.Id));
        }

        request.DbContext.RepairParts.Remove(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);
        
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