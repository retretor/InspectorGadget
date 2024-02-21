using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.Authorization.RemindLogin;

public class RemindLoginQuery : IRequest<(Result, string?)>
{
    public string TelephoneNumber { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
}

public class RemindLoginHandler : IRequestHandler<RemindLoginQuery, (Result, string?)>
{
    private readonly IAuthorizationService _authorizationService;

    public RemindLoginHandler(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task<(Result, string?)> Handle(RemindLoginQuery request, CancellationToken cancellationToken)
    {
        return await _authorizationService.RemindLogin(request);
    }
}

public class RemindLoginValidator : AbstractValidator<RemindLoginQuery>
{
    public RemindLoginValidator()
    {
        RuleFor(x => x.TelephoneNumber).NotEmpty();
        RuleFor(x => x.SecretKey).NotEmpty();
    }
}