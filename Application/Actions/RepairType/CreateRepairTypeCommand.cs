using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairType;

public class CreateRepairTypeCommand : IRequest<(Result, int?)>
{
    public string Name { get; init; } = null!;
}

public class CreateRepairTypeHandler : BaseHandler, IRequestHandler<CreateRepairTypeCommand, (Result, int?)>
{
    public CreateRepairTypeHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<(Result, int?)> Handle(CreateRepairTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = Mapper!.Map<Domain.Entities.Basic.RepairType>(request);
        DbContext.RepairTypes.Add(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
        return (Result.Success(), entity.EntityId);
    }
}

public class CreateRepairTypeValidator : AbstractValidator<CreateRepairTypeCommand>
{
    public CreateRepairTypeValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}