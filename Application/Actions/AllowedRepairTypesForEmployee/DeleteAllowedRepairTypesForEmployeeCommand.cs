using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.AllowedRepairTypesForEmployee;

public class DeleteAllowedRepairTypesForEmployeeCommand : IRequest<Result>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class
    DeleteAllowedRepairTypesForEmployeeHandler : IRequestHandler<DeleteAllowedRepairTypesForEmployeeCommand, Result>
{
    public DeleteAllowedRepairTypesForEmployeeHandler()
    {
    }

    public async Task<Result> Handle(DeleteAllowedRepairTypesForEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }

        var entity = await request.DbContext.AllowedRepairTypesForEmployees.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(AllowedRepairTypesForEmployee), request.Id));
        }

        request.DbContext.AllowedRepairTypesForEmployees.Remove(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);

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