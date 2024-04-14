using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairTypesList;

public class DeleteRepairTypesListCommand : IRequest<Result>
{
    public int Id { get; init; }
}

public class DeleteRepairTypesListHandler : BaseHandler, IRequestHandler<DeleteRepairTypesListCommand, Result>
{
    public DeleteRepairTypesListHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result> Handle(DeleteRepairTypesListCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.RepairTypesLists.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(RepairTypesList), request.Id));
        }

        DbContext.RepairTypesLists.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class DeleteRepairTypesListValidator : AbstractValidator<DeleteRepairTypesListCommand>
{
    public DeleteRepairTypesListValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}