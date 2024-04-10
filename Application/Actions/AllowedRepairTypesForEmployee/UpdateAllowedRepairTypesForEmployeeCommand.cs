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
    public IApplicationDbContext? DbContext { get; set; }
}

public class
    UpdateAllowedRepairTypesForEmployeeHandler : IRequestHandler<UpdateAllowedRepairTypesForEmployeeCommand, Result>
{
    private readonly IMapper _mapper;

    public UpdateAllowedRepairTypesForEmployeeHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateAllowedRepairTypesForEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        if (request.DbContext == null)
        {
            return Result.Failure(new InvalidDbContextException());
        }

        var entity = await request.DbContext.AllowedRepairTypesForEmployees.FindAsync(request.EntityId);
        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(AllowedRepairTypesForEmployee), request.EntityId));
        }

        _mapper.Map(request, entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);

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