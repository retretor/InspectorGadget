using Application.Actions.Authorization;
using Application.Common.Authorization;
using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IAuthorizationService
{
    Task<(Result, AuthorizeResponse?)> AuthorizeByLogin(AuthorizeByLoginQuery request);
    Task<(Result, AuthorizeResponse?)> AuthorizeByToken(AuthorizeByTokenQuery request);
}