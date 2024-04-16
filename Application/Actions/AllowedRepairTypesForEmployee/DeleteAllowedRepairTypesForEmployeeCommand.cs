using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.AllowedRepairTypesForEmployee;

public class DeleteAllowedRepairTypesForEmployeeCommand : IRequest<Result>
{
    public int Id { get; init; }
}

public class
    DeleteAllowedRepairTypesForEmployeeHandler : BaseHandler,
    IRequestHandler<DeleteAllowedRepairTypesForEmployeeCommand, Result>
{
    public DeleteAllowedRepairTypesForEmployeeHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result> Handle(DeleteAllowedRepairTypesForEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await DbContext.AllowedRepairTypesForEmployees.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(AllowedRepairTypesForEmployee), request.Id));
        }

        DbContext.AllowedRepairTypesForEmployees.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class
    DeleteAllowedRepairTypesForEmployeeValidator : AbstractValidator<DeleteAllowedRepairTypesForEmployeeCommand>
{
    public DeleteAllowedRepairTypesForEmployeeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}