using System.IdentityModel.Tokens.Jwt;
using Application.Actions.Authorization;
using Application.Common.Authorization;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.Basic;

namespace Infrastructure.Authorization;

// TODO: ADD Authorization
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

        var dbUser = new DbUser(user.Id, user.FirstName, user.SecondName, user.TelephoneNumber, user.Login,
            user.PasswordHash, user.Role);

        return (Result.Success(), GenerateResponse(dbUser, validTo));
    }

    private AuthorizeResponse? GenerateResponse(DbUser? dbUser, DateTime? validTo = null)
    {
        if (dbUser == null) return null;
        var token = _jwtService.GenerateToken(dbUser, validTo);
        return new AuthorizeResponse(token, dbUser);
    }
}