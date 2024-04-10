using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairTypesList;

public class UpdateRepairTypesListCommand : IRequest<Result>
{
    public int EntityId { get; init; }
    public int RepairTypeForDeviceId { get; init; }
    public int RepairRequestId { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class UpdateRepairTypesListHandler : IRequestHandler<UpdateRepairTypesListCommand, Result>
{
    private readonly IMapper _mapper;

    public UpdateRepairTypesListHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateRepairTypesListCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }
        var entity = await request.DbContext.RepairTypesLists.FindAsync(request.EntityId);

        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.RepairTypesList), request.EntityId));
        }
        
        _mapper.Map(request, entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}

public class UpdateRepairTypesListValidator : AbstractValidator<UpdateRepairTypesListCommand>
{
    public UpdateRepairTypesListValidator()
    {
        RuleFor(x => x.EntityId).NotEmpty();
        RuleFor(x => x.RepairTypeForDeviceId).NotEmpty();
        RuleFor(x => x.RepairRequestId).NotEmpty();
    }
}