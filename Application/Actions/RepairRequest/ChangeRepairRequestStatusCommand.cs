using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.RepairRequest;

public class ChangeRepairRequestStatusCommand : IRequest<(Result, bool)>
{
    public int EntityId { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class ChangeRepairRequestStatusHandler : IRequestHandler<ChangeRepairRequestStatusCommand, (Result, bool)>
{
    private readonly IMapper _mapper;

    public ChangeRepairRequestStatusHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<(Result, bool)> Handle(ChangeRepairRequestStatusCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), false);
        }

        var entity = await Task.Run(() =>
            request.DbContext.ChangeRepairRequestStatus(request.EntityId, request.Status, request.Date)
                .SingleOrDefaultAsync());

        return entity == null
            ? (Result.Failure(new NotFoundException(nameof(RepairRequest), request.EntityId)), false)
            : (Result.Success(), true);
    }
}

public class ChangeRepairRequestStatusValidator : AbstractValidator<ChangeRepairRequestStatusCommand>
{
    public ChangeRepairRequestStatusValidator()
    {
        RuleFor(x => x.EntityId).NotEmpty();
        RuleFor(x => x.Status).NotEmpty().IsEnumName(typeof(RequestStatus));
        RuleFor(x => x.Date).NotEmpty();
    }
}