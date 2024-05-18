using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairType;

public class DeleteRepairTypeCommand : IRequest<Result>
{
    public int Id { get; init; }
}

public class DeleteRepairTypeHandler : BaseHandler, IRequestHandler<DeleteRepairTypeCommand, Result>
{
    public DeleteRepairTypeHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result> Handle(DeleteRepairTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.RepairTypes.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(RepairType), request.Id));
        }

        DbContext.RepairTypes.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class DeleteRepairTypeValidator : AbstractValidator<DeleteRepairTypeCommand>
{
    public DeleteRepairTypeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}