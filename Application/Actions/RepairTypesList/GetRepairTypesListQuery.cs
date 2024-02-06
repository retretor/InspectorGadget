using Application.Common.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairTypesList;

public class GetRepairTypesListQuery : IRequest<Domain.Entities.Basic.RepairTypesList>
{
    public int Id { get; init; }
    public int RepairTypeForDeviceId { get; init; }
    public int RepairRequestId { get; init; }
}

public class GetRepairTypesListQueryHandler : IRequestHandler<GetRepairTypesListQuery, Domain.Entities.Basic.RepairTypesList>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRepairTypesListQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Domain.Entities.Basic.RepairTypesList> Handle(GetRepairTypesListQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.RepairTypesLists.FindAsync(request.Id);
        return _mapper.Map<Domain.Entities.Basic.RepairTypesList>(entity);
    }
}

public class GetRepairTypesListQueryValidator : AbstractValidator<GetRepairTypesListQuery>
{
    public GetRepairTypesListQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RepairTypeForDeviceId).NotEmpty();
        RuleFor(x => x.RepairRequestId).NotEmpty();
    }
}