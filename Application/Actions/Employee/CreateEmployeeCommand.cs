using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Actions.Employee;

public class CreateEmployeeCommand : IRequest<(Result, int?)>
{
    public string FirstName { get; set; } = null!;
    public string SecondName { get; set; } = null!;
    public string TelephoneNumber { get; set; } = null!;
    public int ExperienceYears { get; set; }
    public int YearsInCompany { get; set; }
    public int Rating { get; set; }
    public string Login { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public string Role { get; set; } = null!;
}

public class CreateEmployeeHandler : BaseHandler, IRequestHandler<CreateEmployeeCommand, (Result, int?)>
{
    public CreateEmployeeHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, int?)> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var dbUserId = await Task.Run(() => DbContext.CreateEmployee(request.FirstName, request.SecondName,
            request.TelephoneNumber, request.ExperienceYears, request.YearsInCompany, request.Rating, request.Login,
            request.PasswordHash, request.SecretKey, request.Role).SingleOrDefault(), cancellationToken);
        return dbUserId == null
            ? (Result.Failure(new NotFoundException(nameof(Domain.Entities.Basic.Employee), 0)), null)
            : (Result.Success(), dbUserId.Result);
    }
}

public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.SecondName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TelephoneNumber).NotEmpty()
            .Must(x => x.StartsWith("0") && x.Length == 10 && x.All(char.IsDigit));
        RuleFor(x => x.ExperienceYears).GreaterThanOrEqualTo(0);
        RuleFor(x => x.YearsInCompany).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Rating).InclusiveBetween(0, 10);
        RuleFor(x => x.Login).NotEmpty().MaximumLength(255);
        RuleFor(x => x.PasswordHash).NotEmpty().MaximumLength(255);
        RuleFor(x => x.SecretKey).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Role).NotEmpty().IsEnumName(typeof(Role));
    }
}