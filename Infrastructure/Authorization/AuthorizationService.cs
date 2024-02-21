using System.IdentityModel.Tokens.Jwt;
using Application.Actions.Authorization.AuthorizeByLogin;
using Application.Actions.Authorization.AuthorizeByToken;
using Application.Actions.Authorization.ChangePassword;
using Application.Actions.Authorization.RemindLogin;
using Application.Common.Authorization;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.Basic;

namespace Infrastructure.Authorization;

public class AuthorizationService : IAuthorizationService
{
    private readonly IIdentityService _identityService;
    private readonly JwtService _jwtService;

    public AuthorizationService(IIdentityService identityService, JwtService jwtService)
    {
        _identityService = identityService;
        _jwtService = jwtService;
    }

    public async Task<(Result, AuthorizeResponse?)> AuthorizeByLogin(AuthorizeByLoginQuery request)
    {
        var (result, dbUser) = await _identityService.AuthenticateUser(request.Login, request.Password);

        if (result.Succeeded == false)
        {
            return (result, null);
        }

        var response = GenerateResponse(dbUser);
        return (result, response);
    }

    public async Task<(Result, AuthorizeResponse?)> AuthorizeByToken(AuthorizeByTokenQuery request)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = request.Token;

        if (!_jwtService.IsValidToken(token) || !tokenHandler.CanReadToken(token))
        {
            return (Result.Failure(new List<ResultError> { new(ResultErrorEnum.InvalidToken) }), null);
        }

        var jwtToken = tokenHandler.ReadJwtToken(token);
        var userLoginClaim =
            jwtToken.Claims.First(claim => claim.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
        var validTo = jwtToken.ValidTo;
        var user = await _identityService.GetUserByLogin(userLoginClaim.Value);

        if (user == null)
        {
            return (Result.Failure(new List<ResultError> { new(ResultErrorEnum.UserNotFound) }), null);
        }

        return (Result.Success(), GenerateResponse(user, validTo));
    }

    public async Task<(Result, AuthorizeResponse?)> ChangePassword(ChangePasswordQuery request)
    {
        var (result, dbUser) = await _identityService.AuthenticateUser(request.Login, request.OldPasswordHash);

        if (result.Succeeded == false)
        {
            return (result, null);
        }

        (result, dbUser) = await _identityService.ChangePassword(dbUser, request.NewPasswordHash);

        if (result.Succeeded == false)
        {
            return (result, null);
        }

        var response = GenerateResponse(dbUser);

        return (result, response);
    }

    public async Task<(Result, string?)> RemindLogin(RemindLoginQuery request)
    {
        var (result, dbUser) = await _identityService.GetUserByTelephone(request.TelephoneNumber, request.SecretKey);

        if (result.Succeeded == false)
        {
            return (result, null);
        }

        return dbUser == null
            ? (Result.Failure(new NotFoundException(nameof(DbUser), request.TelephoneNumber)), null)
            : (result, dbUser.Login);
    }

    private AuthorizeResponse? GenerateResponse(DbUser? dbUser, DateTime? validTo = null)
    {
        if (dbUser == null) return null;
        var token = _jwtService.GenerateToken(dbUser, validTo);
        return new AuthorizeResponse(token, dbUser);
    }
}