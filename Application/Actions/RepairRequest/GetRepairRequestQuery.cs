using Application.Common.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairRequest;

public class GetRepairRequestQuery : IRequest<Domain.Entities.Basic.RepairRequest>
{
    public int Id { get; init; }
    public int DeviceId { get; init; }
    public int ClientId { get; init; }
    public int EmployeeId { get; init; }
    public string SerialNumber { get; init; }
}

public class GetRepairRequestQueryHandler : IRequestHandler<GetRepairRequestQuery, Domain.Entities.Basic.RepairRequest>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRepairRequestQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Domain.Entities.Basic.RepairRequest> Handle(GetRepairRequestQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.RepairRequests.FindAsync(request.Id);
        return _mapper.Map<Domain.Entities.Basic.RepairRequest>(entity);
    }
}

public class GetRepairRequestQueryValidator : AbstractValidator<GetRepairRequestQuery>
{
    public GetRepairRequestQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DeviceId).NotEmpty();
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.SerialNumber).NotEmpty().MaximumLength(20);
    }
}