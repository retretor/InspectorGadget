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
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string Specification { get; init; } = null!;
    public int CurrentCount { get; init; }
    public int MinAllowedCount { get; init; }
    public float Cost { get; init; }
    public string Condition { get; init; } = null!;
    public IApplicationDbContext? DbContext { get; set; }
}

public class UpdateRepairPartHandler : IRequestHandler<UpdateRepairPartCommand, Result>
{
    private readonly IMapper _mapper;

    public UpdateRepairPartHandler(IApplicationDbContext context, IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateRepairPartCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext is null)
        {
            return Result.Failure(new InvalidDbContextException());
        }

        var entity = await request.DbContext.RepairParts.FindAsync(request.Id);

        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.RepairPart), request.Id));
        }

        _mapper.Map(request, entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class UpdateRepairPartValidator : AbstractValidator<UpdateRepairPartCommand>
{
    public UpdateRepairPartValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Specification).NotEmpty();
        RuleFor(x => x.CurrentCount).NotEmpty().GreaterThanOrEqualTo(0);
        RuleFor(x => x.MinAllowedCount).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Cost).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Condition).NotEmpty().IsEnumName(typeof(RepairPartCondition));
    }
}