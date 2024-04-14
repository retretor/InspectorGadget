using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.PartForRepairType;

public class CreatePartForRepairTypeCommand : IRequest<(Result, int?)>
{
    public int PartCount { get; init; }
    public int RepairTypeForDeviceId { get; init; }
    public int RepairPartId { get; init; }
}

public class CreatePartForRepairTypeHandler : BaseHandler,
    IRequestHandler<CreatePartForRepairTypeCommand, (Result, int?)>
{
    public CreatePartForRepairTypeHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<(Result, int?)> Handle(CreatePartForRepairTypeCommand request,
        CancellationToken cancellationToken)
    {
        var entity = Mapper!.Map<Domain.Entities.Basic.PartForRepairType>(request);
        DbContext.PartForRepairTypes.Add(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
        return (Result.Success(), entity.EntityId);
    }
}

public class CreatePartForRepairTypeValidator : AbstractValidator<CreatePartForRepairTypeCommand>
{
    public CreatePartForRepairTypeValidator()
    {
        RuleFor(x => x.PartCount).NotEmpty().GreaterThan(0);
        RuleFor(x => x.RepairTypeForDeviceId).NotEmpty();
        RuleFor(x => x.RepairPartId).NotEmpty();
    }
}