using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.AllowedRepairTypesForEmployee;

public class UpdateAllowedRepairTypesForEmployeeQuery : IRequest
{
    public int Id { get; init; }
    public int EmployeeId { get; init; }
    public int RepairTypeId { get; init; }
}

public class UpdateAllowedRepairTypesForEmployeeHandler : IRequestHandler<UpdateAllowedRepairTypesForEmployeeQuery>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateAllowedRepairTypesForEmployeeHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task Handle(UpdateAllowedRepairTypesForEmployeeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.AllowedRepairTypesForEmployees.FindAsync(request.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(AllowedRepairTypesForEmployee), request.Id);
        }
        
        _mapper.Map(request, entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class UpdateAllowedRepairTypesForEmployeeValidator : AbstractValidator<UpdateAllowedRepairTypesForEmployeeQuery>
{
    public UpdateAllowedRepairTypesForEmployeeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RepairTypeId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
    }
}