using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Actions.RequestStatusHistory;

public class CreateRequestStatusHistoryCommand : IRequest<(Result, int?)>
{
    public DateTime Date { get; init; }
    public int RepairRequestId { get; init; }
    public string RequestStatus { get; init; } = null!;
}

public class CreateRequestStatusHistoryHandler : BaseHandler,
    IRequestHandler<CreateRequestStatusHistoryCommand, (Result, int?)>
{
    public CreateRequestStatusHistoryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<(Result, int?)> Handle(CreateRequestStatusHistoryCommand request,
        CancellationToken cancellationToken)
    {
        var entity = Mapper!.Map<Domain.Entities.Basic.RequestStatusHistory>(request);
        DbContext.RequestStatusHistories.Add(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
        return (Result.Success(), entity.EntityId);
    }
}

public class CreateRequestStatusHistoryValidator : AbstractValidator<CreateRequestStatusHistoryCommand>
{
    public CreateRequestStatusHistoryValidator()
    {
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.RepairRequestId).NotEmpty();
        RuleFor(x => x.RequestStatus).NotEmpty().IsEnumName(typeof(RequestStatus));
    }
}