using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.Employee;

public class DeleteEmployeeCommand : IRequest<Result>
{
    public int Id { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, Result>
{
    public DeleteEmployeeHandler()
    {
    }

    public async Task<Result> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }

        var entity = await request.DbContext.Employees.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.Employee), request.Id));
        }

        request.DbContext.Employees.Remove(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}

public class DeleteEmployeeValidator : AbstractValidator<DeleteEmployeeCommand>
{
    public DeleteEmployeeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}