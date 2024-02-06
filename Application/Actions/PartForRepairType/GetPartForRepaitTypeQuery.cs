using Application.Common.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.PartForRepairType;

public class GetPartForRepairTypeQuery : IRequest<Domain.Entities.Basic.PartForRepairType>
{
    public int Id { get; init; }
    public int PartCount { get; init; }
    public int RepairTypeForDeviceId { get; init; }
    public int RepairPartId { get; init; }
}

public class GetPartForRepairTypeQueryHandler : IRequestHandler<GetPartForRepairTypeQuery, Domain.Entities.Basic.PartForRepairType>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPartForRepairTypeQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Domain.Entities.Basic.PartForRepairType> Handle(GetPartForRepairTypeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.PartForRepairTypes.FindAsync(request.Id);
        return _mapper.Map<Domain.Entities.Basic.PartForRepairType>(entity);
    }
}

public class GetPartForRepairTypeQueryValidator : AbstractValidator<GetPartForRepairTypeQuery>
{
    public GetPartForRepairTypeQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.PartCount).NotEmpty().GreaterThan(0);
        RuleFor(x => x.RepairTypeForDeviceId).NotEmpty();
        RuleFor(x => x.RepairPartId).NotEmpty();
    }
}