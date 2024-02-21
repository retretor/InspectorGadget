using Application.Common.Authorization;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Actions.Authorization.ChangePassword;

public class ChangePasswordQuery : IRequest<(Result, AuthorizeResponse?)>
{
    public string Login { get; init; } = null!;
    public string OldPasswordHash { get; init; } = null!;
    public string NewPasswordHash { get; init; } = null!;
}

public class ChangePasswordHandler : IRequestHandler<ChangePasswordQuery, (Result, AuthorizeResponse?)>
{
    private readonly IAuthorizationService _authorizationService;

    public ChangePasswordHandler(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task<(Result, AuthorizeResponse?)> Handle(ChangePasswordQuery request,
        CancellationToken cancellationToken)
    {
        return await _authorizationService.ChangePassword(request);
    }
}

public class ChangePasswordValidator : AbstractValidator<ChangePasswordQuery>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.Login).NotEmpty();
        RuleFor(x => x.OldPasswordHash).NotEmpty();
        RuleFor(x => x.NewPasswordHash).NotEmpty();
    }
}