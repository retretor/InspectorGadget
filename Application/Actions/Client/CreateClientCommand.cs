using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.Responses;
using FluentValidation;
using MediatR;

namespace Application.Actions.Client;

public class CreateClientCommand : IRequest<(Result, CreateClientResponse?)>
{
    public string FirstName { get; init; } = null!;
    public string SecondName { get; init; } = null!;
    public string TelephoneNumber { get; init; } = null!;
    public int DiscountPercentage { get; init; } = 0;
    public string Login { get; init; } = null!;
    public string PasswordHash { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
}

public class CreateClientHandler : BaseHandler, IRequestHandler<CreateClientCommand, (Result, CreateClientResponse?)>
{
    public CreateClientHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(Result, CreateClientResponse?)> Handle(CreateClientCommand request,
        CancellationToken cancellationToken)
    {
        var dbUserId = await Task.Run(() => DbContext.CreateClient(request.FirstName, request.SecondName,
            request.TelephoneNumber, request.DiscountPercentage, request.Login, request.PasswordHash,
            request.SecretKey).SingleOrDefault(), cancellationToken);
        return dbUserId == null
            ? (Result.Failure(new NotFoundException(nameof(Client), 0)), null)
            : (Result.Success(), new CreateClientResponse { DbUserId = dbUserId.Result });
    }
}

public class CreateClientValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.SecondName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TelephoneNumber).NotEmpty()
            .Must(x => x.StartsWith("0") && x.Length == 10 && x.All(char.IsDigit));
        RuleFor(x => x.DiscountPercentage).InclusiveBetween(0, 100);
        RuleFor(x => x.Login).NotEmpty().MaximumLength(255);
        RuleFor(x => x.PasswordHash).NotEmpty().MaximumLength(255);
        RuleFor(x => x.SecretKey).NotEmpty().MaximumLength(255);
    }
}