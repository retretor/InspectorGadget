using Application.Common.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.AllowedRepairTypesForEmployee;

public class CreateAllowedRepairTypesForEmployeeQuery : IRequest<int>
{
    public int EmployeeId { get; init; }
    public int RepairTypeId { get; init; }
}

public class CreateAllowedRepairTypesForEmployeeHandler : IRequestHandler<CreateAllowedRepairTypesForEmployeeQuery, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateAllowedRepairTypesForEmployeeHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateAllowedRepairTypesForEmployeeQuery request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Basic.AllowedRepairTypesForEmployee>(request);
        _context.AllowedRepairTypesForEmployees.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public class CreateAllowedRepairTypesForEmployeeValidator : AbstractValidator<CreateAllowedRepairTypesForEmployeeQuery>
{
    public CreateAllowedRepairTypesForEmployeeValidator()
    {
        RuleFor(x => x.RepairTypeId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
    }
}