using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairRequest;

public class UpdateRepairRequestCommand : IRequest<Result>
{
    public int EntityId { get; init; }
    public int DeviceId { get; init; }
    public int ClientId { get; init; }
    public int EmployeeId { get; init; }
    public string SerialNumber { get; init; } = null!;
    public string Description { get; init; } = null!;
}

public class UpdateRepairRequestHandler : BaseHandler, IRequestHandler<UpdateRepairRequestCommand, Result>
{
    public UpdateRepairRequestHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<Result> Handle(UpdateRepairRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.RepairRequests.FindAsync(request.EntityId);

        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(RepairRequest), request.EntityId));
        }

        Mapper!.Map(request, entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class UpdateRepairRequestValidator : AbstractValidator<UpdateRepairRequestCommand>
{
    public UpdateRepairRequestValidator()
    {
        RuleFor(x => x.EntityId).NotEmpty();
        RuleFor(x => x.DeviceId).NotEmpty();
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.SerialNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Description).NotEmpty();
    }
}