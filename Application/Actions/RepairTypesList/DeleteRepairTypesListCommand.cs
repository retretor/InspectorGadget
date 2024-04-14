using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairTypesList;

public class DeleteRepairTypesListCommand : IRequest<Result>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class DeleteRepairTypesListHandler : IRequestHandler<DeleteRepairTypesListCommand, Result>
{
    public async Task<Result> Handle(DeleteRepairTypesListCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }
        var entity = await request.DbContext.RepairTypesLists.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(RepairTypesList), request.Id));
        }

        request.DbContext.RepairTypesLists.Remove(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);
        
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