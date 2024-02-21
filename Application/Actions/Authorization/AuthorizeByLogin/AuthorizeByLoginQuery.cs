using Application.Common.Authorization;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.Authorization.AuthorizeByLogin;

public class AuthorizeByLoginQuery : IRequest<(Result, AuthorizeResponse?)>
{
    public string Login { get; init; } = null!;
    public string Password { get; init; } = null!;
}

public class AuthorizeByLoginHandler : IRequestHandler<AuthorizeByLoginQuery, (Result, AuthorizeResponse?)>
{
    private readonly IAuthorizationService _authorizationService;

    public AuthorizeByLoginHandler(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task<(Result, AuthorizeResponse?)> Handle(AuthorizeByLoginQuery request,
        CancellationToken cancellationToken)
    {
        return await _authorizationService.AuthorizeByLogin(request);
    }
}

public class AuthorizeByLoginValidator : AbstractValidator<AuthorizeByLoginQuery>
{
    public AuthorizeByLoginValidator()
    {
        RuleFor(x => x.Login).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}