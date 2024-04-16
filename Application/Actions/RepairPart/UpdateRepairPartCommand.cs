using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairPart;

public class UpdateRepairPartCommand : IRequest<Result>
{
    public int EntityId { get; init; }
    public string Name { get; init; } = null!;
    public string Specification { get; init; } = null!;
    public int CurrentCount { get; init; }
    public int MinAllowedCount { get; init; }
    public float Cost { get; init; }
    public string Condition { get; init; } = null!;
}

public class UpdateRepairPartHandler : BaseHandler, IRequestHandler<UpdateRepairPartCommand, Result>
{
    public UpdateRepairPartHandler(IApplicationDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<Result> Handle(UpdateRepairPartCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.RepairParts.FindAsync(request.EntityId);

        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.RepairPart), request.EntityId));
        }

        Mapper!.Map(request, entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class UpdateRepairPartValidator : AbstractValidator<UpdateRepairPartCommand>
{
    public UpdateRepairPartValidator()
    {
        RuleFor(x => x.EntityId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Specification).NotEmpty();
        RuleFor(x => x.CurrentCount).NotEmpty().GreaterThanOrEqualTo(0);
        RuleFor(x => x.MinAllowedCount).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Cost).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Condition).NotEmpty().IsEnumName(typeof(RepairPartCondition));
    }
}