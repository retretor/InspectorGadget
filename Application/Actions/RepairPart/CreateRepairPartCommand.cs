using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairPart;

public class CreateRepairPartCommand : IRequest<(Result, int?)>
{
    public string Name { get; init; } = null!;
    public string Specification { get; init; } = null!;
    public int CurrentCount { get; init; }
    public int MinAllowedCount { get; init; }
    public float Cost { get; init; }
    public string Condition { get; init; } = null!;
    public IApplicationDbContext? DbContext { get; set; }
}

public class CreateRepairPartHandler : IRequestHandler<CreateRepairPartCommand, (Result, int?)>
{
    private readonly IMapper _mapper;

    public CreateRepairPartHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<(Result, int?)> Handle(CreateRepairPartCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        var entity = _mapper.Map<Domain.Entities.Basic.RepairPart>(request);
        request.DbContext.RepairParts.Add(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);
        return (Result.Success(), entity.Id);
    }
}

public class CreateRepairPartValidator : AbstractValidator<CreateRepairPartCommand>
{
    public CreateRepairPartValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Specification).NotEmpty();
        RuleFor(x => x.CurrentCount).NotEmpty().GreaterThanOrEqualTo(0);
        RuleFor(x => x.MinAllowedCount).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Cost).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Condition).NotEmpty().IsEnumName(typeof(RepairPartCondition));
    }
}