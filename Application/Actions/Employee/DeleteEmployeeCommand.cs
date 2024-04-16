using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.Employee;

public class DeleteEmployeeCommand : IRequest<Result>
{
    public int Id { get; init; }
}

public class DeleteEmployeeHandler : BaseHandler, IRequestHandler<DeleteEmployeeCommand, Result>
{
    public DeleteEmployeeHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.Employees.FindAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.Employee), request.Id));
        }

        DbContext.Employees.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);

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