using Application.Common.Interfaces;
using AutoMapper;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Actions.RequestStatusHistory;

public class GetRequestStatusHistoryQuery : IRequest<Domain.Entities.Basic.RequestStatusHistory>
{
    public int Id { get; init; }
    public DateTime Date { get; init; }
    public int RepairRequestId { get; init; }
    public RequestStatus RequestStatus { get; init; }
}

public class GetRequestStatusHistoryQueryHandler : IRequestHandler<GetRequestStatusHistoryQuery,
    Domain.Entities.Basic.RequestStatusHistory>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRequestStatusHistoryQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Domain.Entities.Basic.RequestStatusHistory> Handle(GetRequestStatusHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.RequestStatusHistories.FindAsync(request.Id);
        return _mapper.Map<Domain.Entities.Basic.RequestStatusHistory>(entity);
    }
}

public class GetRequestStatusHistoryQueryValidator : AbstractValidator<GetRequestStatusHistoryQuery>
{
    public GetRequestStatusHistoryQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.RepairRequestId).NotEmpty();
        RuleFor(x => x.RequestStatus).NotEmpty();
    }
}