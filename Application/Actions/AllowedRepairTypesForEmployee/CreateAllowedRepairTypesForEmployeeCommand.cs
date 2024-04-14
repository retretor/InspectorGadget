using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.AllowedRepairTypesForEmployee;

public class CreateAllowedRepairTypesForEmployeeCommand : IRequest<(Result, int?)>
{
    public int EmployeeId { get; init; }
    public int RepairTypeId { get; init; }
}

public class CreateAllowedRepairTypesForEmployeeHandler : BaseHandler,
    IRequestHandler<CreateAllowedRepairTypesForEmployeeCommand, (Result, int?)>
{
    public CreateAllowedRepairTypesForEmployeeHandler(IApplicationDbContext dbContext, IMapper mapper) : base(
        dbContext, mapper)
    {
    }

    public async Task<(Result, int?)> Handle(CreateAllowedRepairTypesForEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var entity = Mapper!.Map<Domain.Entities.Basic.AllowedRepairTypesForEmployee>(request);
        DbContext.AllowedRepairTypesForEmployees.Add(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
        return (Result.Success(), entity.EntityId);
    }
}

public class
    CreateAllowedRepairTypesForEmployeeValidator : AbstractValidator<CreateAllowedRepairTypesForEmployeeCommand>
{
    public CreateAllowedRepairTypesForEmployeeValidator()
    {
        RuleFor(x => x.RepairTypeId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
    }
}