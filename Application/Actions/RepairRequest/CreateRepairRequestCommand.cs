using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairRequest;

public class CreateRepairRequestCommand : IRequest<(Result, int?)>
{
    public int DeviceId { get; init; }
    public int ClientId { get; init; }
    public int EmployeeId { get; init; }
    public string SerialNumber { get; init; } = null!;
    public string Description { get; init; } = null!;
    public IApplicationDbContext? DbContext { get; set; }
}

public class CreateRepairRequestHandler : IRequestHandler<CreateRepairRequestCommand, (Result, int?)>
{
    private readonly IMapper _mapper;

    public CreateRepairRequestHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<(Result, int?)> Handle(CreateRepairRequestCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entity = _mapper.Map<Domain.Entities.Basic.RepairRequest>(request);
        request.DbContext.RepairRequests.Add(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);
        return (Result.Success(), entity.EntityId);
    }
}

public class CreateRepairRequestValidator : AbstractValidator<CreateRepairRequestCommand>
{
    public CreateRepairRequestValidator()
    {
        RuleFor(x => x.DeviceId).NotEmpty();
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.SerialNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Description).NotEmpty();
    }
}