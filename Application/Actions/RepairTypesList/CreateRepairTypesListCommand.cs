using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairTypesList;

public class CreateRepairTypesListCommand : IRequest<(Result, int?)>
{
    public int RepairTypeForDeviceId { get; init; }
    public int RepairRequestId { get; init; }
}

public class CreateRepairTypesListHandler : BaseHandler, IRequestHandler<CreateRepairTypesListCommand, (Result, int?)>
{
    public CreateRepairTypesListHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<(Result, int?)> Handle(CreateRepairTypesListCommand request, CancellationToken cancellationToken)
    {
        var entity = Mapper!.Map<Domain.Entities.Basic.RepairTypesList>(request);
        DbContext.RepairTypesLists.Add(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
        return (Result.Success(), entity.EntityId);
    }
}

public class CreateRepairTypesListValidator : AbstractValidator<CreateRepairTypesListCommand>
{
    public CreateRepairTypesListValidator()
    {
        RuleFor(x => x.RepairTypeForDeviceId).NotEmpty();
        RuleFor(x => x.RepairRequestId).NotEmpty();
    }
}