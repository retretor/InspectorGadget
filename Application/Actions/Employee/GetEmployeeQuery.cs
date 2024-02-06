using Application.Common.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.Employee;

public class GetEmployeeQuery : IRequest<Domain.Entities.Basic.Employee>
{
    public int Id { get; init; }
    public int DbUserId { get; init; }
}

public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, Domain.Entities.Basic.Employee>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEmployeeQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Domain.Entities.Basic.Employee> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Employees.FindAsync(request.Id);
        return _mapper.Map<Domain.Entities.Basic.Employee>(entity);
    }
}

public class GetEmployeeQueryValidator : AbstractValidator<GetEmployeeQuery>
{
    public GetEmployeeQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DbUserId).NotEmpty();
    }
}