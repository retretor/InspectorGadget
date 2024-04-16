using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.PartForRepairType;

public class DeletePartForRepairTypeCommand : IRequest<Result>
{
    public int Id { get; init; }
}

public class DeletePartForRepairTypeHandler : BaseHandler, IRequestHandler<DeletePartForRepairTypeCommand, Result>
{
    public DeletePartForRepairTypeHandler(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<Result> Handle(DeletePartForRepairTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.PartForRepairTypes.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.PartForRepairType), request.Id));
        }

        DbContext.PartForRepairTypes.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);

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