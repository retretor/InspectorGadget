using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.PartForRepairType;

public class UpdatePartForRepairTypeCommand : IRequest<Result>
{
    public int EntityId { get; init; }
    public int PartCount { get; init; }
    public int RepairTypeForDeviceId { get; init; }
    public int RepairPartId { get; init; }
    public IApplicationDbContext? DbContext { get; set; }
}

public class UpdatePartForRepairTypeHandler : IRequestHandler<UpdatePartForRepairTypeCommand, Result>
{
    private readonly IMapper _mapper;

    public UpdatePartForRepairTypeHandler(IApplicationDbContext context, IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdatePartForRepairTypeCommand request, CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }

        var entity = await request.DbContext.PartForRepairTypes.FindAsync(request.EntityId);

        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.PartForRepairType), request.EntityId));
        }

        _mapper.Map(request, entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

public class UpdatePartForRepairTypeValidator : AbstractValidator<UpdatePartForRepairTypeCommand>
{
    public UpdatePartForRepairTypeValidator()
    {
        RuleFor(x => x.EntityId).NotEmpty();
        RuleFor(x => x.PartCount).NotEmpty().GreaterThan(0);
        RuleFor(x => x.RepairTypeForDeviceId).NotEmpty();
        RuleFor(x => x.RepairPartId).NotEmpty();
    }
}