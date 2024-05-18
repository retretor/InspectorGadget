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
}

public class UpdatePartForRepairTypeHandler : BaseHandler, IRequestHandler<UpdatePartForRepairTypeCommand, Result>
{
    public UpdatePartForRepairTypeHandler(IApplicationDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<Result> Handle(UpdatePartForRepairTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.PartForRepairTypes.FindAsync(request.EntityId);

        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(PartForRepairType),
                request.EntityId));
        }

        Mapper!.Map(request, entity);
        await DbContext.SaveChangesAsync(cancellationToken);
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