using Application.Common.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairType;

public class GetRepairTypeQuery : IRequest<Domain.Entities.Basic.RepairType>
{
    public int Id { get; init; }
    public string Name { get; init; }
}

public class GetRepairTypeQueryHandler : IRequestHandler<GetRepairTypeQuery, Domain.Entities.Basic.RepairType>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRepairTypeQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Domain.Entities.Basic.RepairType> Handle(GetRepairTypeQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.RepairTypes.FindAsync(request.Id);
        return _mapper.Map<Domain.Entities.Basic.RepairType>(entity);
    }
}

public class GetRepairTypeQueryValidator : AbstractValidator<GetRepairTypeQuery>
{
    public GetRepairTypeQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}