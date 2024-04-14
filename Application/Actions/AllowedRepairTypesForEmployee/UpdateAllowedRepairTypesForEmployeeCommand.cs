using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.AllowedRepairTypesForEmployee;

public class UpdateAllowedRepairTypesForEmployeeCommand : IRequest<Result>
{
    public int EntityId { get; init; }
    public int EmployeeId { get; init; }
    public int RepairTypeId { get; init; }
}

public class
    UpdateAllowedRepairTypesForEmployeeHandler : BaseHandler,
    IRequestHandler<UpdateAllowedRepairTypesForEmployeeCommand, Result>
{
    public UpdateAllowedRepairTypesForEmployeeHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext,
        mapper)
    {
    }

    public async Task<Result> Handle(UpdateAllowedRepairTypesForEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await DbContext.AllowedRepairTypesForEmployees.FindAsync(request.EntityId);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(AllowedRepairTypesForEmployee), request.EntityId));
        }

        Mapper!.Map(request, entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class
    UpdateAllowedRepairTypesForEmployeeValidator : AbstractValidator<UpdateAllowedRepairTypesForEmployeeCommand>
{
    public UpdateAllowedRepairTypesForEmployeeValidator()
    {
        RuleFor(x => x.EntityId).NotEmpty();
        RuleFor(x => x.RepairTypeId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
    }
}