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
    public string SerialNumber { get; init; } = null!;
    public string Description { get; init; } = null!;
}

public class CreateRepairRequestHandler : BaseHandler, IRequestHandler<CreateRepairRequestCommand, (Result, int?)>
{
    public CreateRepairRequestHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<(Result, int?)> Handle(CreateRepairRequestCommand request, CancellationToken cancellationToken)
    {
        var requestId = await Task.Run(() => DbContext.CreateRepairRequest(request.DeviceId, request.ClientId,
            request.SerialNumber,
            request.Description).SingleOrDefault(), cancellationToken);
        return requestId == null
            ? (Result.Failure(new NotFoundException(nameof(RepairRequest), 0)), null)
            : (Result.Success(), requestId.Result);
    }
}

public class CreateRepairRequestValidator : AbstractValidator<CreateRepairRequestCommand>
{
    public CreateRepairRequestValidator()
    {
        RuleFor(x => x.DeviceId).NotEmpty();
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.SerialNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Description).NotEmpty();
    }
}