using Application.Common.Exceptions;
using Application.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Actions.AllowedRepairTypesForEmployee;

public class DeleteAllowedRepairTypesForEmployeeQuery : IRequest
{
    public int Id { get; init; }
    
    public DeleteAllowedRepairTypesForEmployeeQuery(int id) => Id = id;
}

public class DeleteAllowedRepairTypesForEmployeeHandler : IRequestHandler<DeleteAllowedRepairTypesForEmployeeQuery>
{
    private readonly IApplicationDbContext _context;

    public DeleteAllowedRepairTypesForEmployeeHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteAllowedRepairTypesForEmployeeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.AllowedRepairTypesForEmployees.FindAsync(request.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(AllowedRepairTypesForEmployee), request.Id);
        }

        _context.AllowedRepairTypesForEmployees.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class DeleteAllowedRepairTypesForEmployeeValidator : AbstractValidator<DeleteAllowedRepairTypesForEmployeeQuery>
{
    public DeleteAllowedRepairTypesForEmployeeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}