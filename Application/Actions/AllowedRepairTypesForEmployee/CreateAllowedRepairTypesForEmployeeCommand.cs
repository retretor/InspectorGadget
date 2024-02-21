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
    public IApplicationDbContext? DbContext { get; set; }
}

public class
    CreateAllowedRepairTypesForEmployeeHandler : IRequestHandler<CreateAllowedRepairTypesForEmployeeCommand, (Result,
    int?)>
{
    private readonly IMapper _mapper;

    public CreateAllowedRepairTypesForEmployeeHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<(Result, int?)> Handle(CreateAllowedRepairTypesForEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Basic.AllowedRepairTypesForEmployee>(request);
        if (request.DbContext == null)
        {
            return (Result.Failure(new InvalidDbContextException()), null);
        }

        request.DbContext.AllowedRepairTypesForEmployees.Add(entity);
        await request.DbContext.SaveChangesAsync(cancellationToken);
        return (Result.Success(), entity.Id);
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