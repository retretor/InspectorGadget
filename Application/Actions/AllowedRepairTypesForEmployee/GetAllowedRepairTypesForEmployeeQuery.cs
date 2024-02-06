using Application.Common.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.AllowedRepairTypesForEmployee;

public class GetAllowedRepairTypesForEmployeeQuery : IRequest<Domain.Entities.Basic.AllowedRepairTypesForEmployee>
{
    public int Id { get; init; }

    public GetAllowedRepairTypesForEmployeeQuery(int id) => Id = id;
}

public class GetAllAllowedRepairTypesForEmployeeQuery : IRequest<IEnumerable<Domain.Entities.Basic.AllowedRepairTypesForEmployee>>
{
}

public class GetAllowedRepairTypesForEmployeeHandler : IRequestHandler<GetAllowedRepairTypesForEmployeeQuery, Domain.Entities.Basic.AllowedRepairTypesForEmployee>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllowedRepairTypesForEmployeeHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Domain.Entities.Basic.AllowedRepairTypesForEmployee> Handle(
        GetAllowedRepairTypesForEmployeeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.AllowedRepairTypesForEmployees.FindAsync(request.Id);
        return _mapper.Map<Domain.Entities.Basic.AllowedRepairTypesForEmployee>(entity);
    }
}

public class GetAllAllowedRepairTypesForEmployeeHandler : IRequestHandler<GetAllAllowedRepairTypesForEmployeeQuery,
    IEnumerable<Domain.Entities.Basic.AllowedRepairTypesForEmployee>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAllowedRepairTypesForEmployeeHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Domain.Entities.Basic.AllowedRepairTypesForEmployee>> Handle(GetAllAllowedRepairTypesForEmployeeQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _context.AllowedRepairTypesForEmployees.ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<Domain.Entities.Basic.AllowedRepairTypesForEmployee>>(entities);
    }
}

public class GetAllowedRepairTypesForEmployeeValidator : AbstractValidator<GetAllowedRepairTypesForEmployeeQuery>
{
    public GetAllowedRepairTypesForEmployeeValidator()
    {
        RuleFor(v => v.Id).NotEmpty().GreaterThanOrEqualTo(0);
    }
}