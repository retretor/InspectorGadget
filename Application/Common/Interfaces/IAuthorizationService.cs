using Application.Actions.Authorization.AuthorizeByLogin;
using Application.Actions.Authorization.AuthorizeByToken;
using Application.Actions.Authorization.ChangePassword;
using Application.Actions.Authorization.RemindLogin;
using Application.Common.Authorization;
using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IAuthorizationService
{
    Task<(Result, AuthorizeResponse?)> AuthorizeByLogin(AuthorizeByLoginQuery request);
    Task<(Result, AuthorizeResponse?)> AuthorizeByToken(AuthorizeByTokenQuery request);
    Task<(Result, AuthorizeResponse?)> ChangePassword(ChangePasswordQuery request);
    Task<(Result, string?)> RemindLogin(RemindLoginQuery request);
}