using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Actions.Employee;

public class UpdateEmployeeCommand : IRequest<Result>
{
    public int EntityId { get; init; }
    public string FirstName { get; set; } = null!;
    public string SecondName { get; set; } = null!;
    public string TelephoneNumber { get; set; } = null!;
    public int ExperienceYears { get; set; }
    public int YearsInCompany { get; set; }
    public int Rating { get; set; }
}

public class UpdateEmployeeHandler : BaseHandler, IRequestHandler<UpdateEmployeeCommand, Result>
{
    public UpdateEmployeeHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<Result> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.Employees.FindAsync(request.EntityId);

        if (entity == null)
        {
            return Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.Employee), request.EntityId));
        }

        Mapper!.Map(request, entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeValidator()
    {
        RuleFor(x => x.EntityId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.SecondName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TelephoneNumber).NotEmpty()
            .Must(x => x.StartsWith("0") && x.Length == 10 && x.All(char.IsDigit));
        RuleFor(x => x.ExperienceYears).GreaterThanOrEqualTo(0);
        RuleFor(x => x.YearsInCompany).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Rating).InclusiveBetween(0, 10);
    }
}