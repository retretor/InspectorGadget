using Application.Common.Authorization;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.Authorization;

public class AuthorizeByTokenQuery : IRequest<(Result, AuthorizeResponse?)>
{
    public string Token { get; init; } = null!;
}

public class AuthorizeByTokenHandler : IRequestHandler<AuthorizeByTokenQuery, (Result, AuthorizeResponse?)>
{
    private readonly IAuthorizationService _authorizationService;

    public AuthorizeByTokenHandler(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task<(Result, AuthorizeResponse?)> Handle(AuthorizeByTokenQuery request, CancellationToken cancellationToken)
    {
        return await _authorizationService.AuthorizeByToken(request);
    }
}

public class AuthorizeByTokenValidator : AbstractValidator<AuthorizeByTokenQuery>
{
    public AuthorizeByTokenValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
    }
}