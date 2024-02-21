using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairType;

public class DeleteRepairTypeCommand : IRequest<Result>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class DeleteRepairTypeHandler : IRequestHandler<DeleteRepairTypeCommand, Result>
{
    public DeleteRepairTypeHandler()
    {
    }

    public async Task<Result> Handle(DeleteRepairTypeCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }

        var entity = await request.DbContext.RepairTypes.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.RepairType), request.Id));
        }

        request.DbContext.RepairTypes.Remove(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);
        
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