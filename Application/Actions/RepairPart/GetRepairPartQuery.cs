using Application.Common.Interfaces;
using AutoMapper;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairPart;

public class GetRepairPartQuery : IRequest<Domain.Entities.Basic.RepairPart>
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string Specification { get; init; } = null!;
    public int CurrentCount { get; init; }
    public int MinAllowedCount { get; init; }
    public float Cost { get; init; }
    public RepairPartCondition Condition { get; init; }
}

public class GetRepairPartQueryHandler : IRequestHandler<GetRepairPartQuery, Domain.Entities.Basic.RepairPart>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRepairPartQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Domain.Entities.Basic.RepairPart> Handle(GetRepairPartQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.RepairParts.FindAsync(request.Id);
        return _mapper.Map<Domain.Entities.Basic.RepairPart>(entity);
    }
}

public class GetRepairPartQueryValidator : AbstractValidator<GetRepairPartQuery>
{
    public GetRepairPartQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Specification).NotEmpty();
        RuleFor(x => x.CurrentCount).NotEmpty().GreaterThanOrEqualTo(0);
        RuleFor(x => x.MinAllowedCount).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Cost).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Condition).NotEmpty();
    }
}