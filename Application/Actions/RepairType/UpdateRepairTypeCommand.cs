using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.RepairType;

public class UpdateRepairTypeCommand : IRequest<Result>
{
    public int EntityId { get; init; }
    public string Name { get; init; } = null!;
}

public class UpdateRepairTypeHandler : BaseHandler, IRequestHandler<UpdateRepairTypeCommand, Result>
{
    public UpdateRepairTypeHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<Result> Handle(UpdateRepairTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.RepairTypes.FindAsync(request.EntityId);

        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(RepairType), request.EntityId));
        }

        Mapper!.Map(request, entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class UpdateRepairTypeValidator : AbstractValidator<UpdateRepairTypeCommand>
{
    public UpdateRepairTypeValidator()
    {
        RuleFor(x => x.EntityId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}