using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.PartForRepairType;

public class DeletePartForRepairTypeCommand : IRequest<Result>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class DeletePartForRepairTypeHandler : IRequestHandler<DeletePartForRepairTypeCommand, Result>
{
    public DeletePartForRepairTypeHandler(IApplicationDbContext context)
    {
    }

    public async Task<Result> Handle(DeletePartForRepairTypeCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }

        var entity = await request.DbContext.PartForRepairTypes.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.PartForRepairType), request.Id));
        }

        request.DbContext.PartForRepairTypes.Remove(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class DeletePartForRepairTypeValidator : AbstractValidator<DeletePartForRepairTypeCommand>
{
    public DeletePartForRepairTypeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}