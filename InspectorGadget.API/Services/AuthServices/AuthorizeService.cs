using System.IdentityModel.Tokens.Jwt;
using InspectorGadget.DTOs;
using InspectorGadget.Services.ModelServices;

namespace InspectorGadget.Services.AuthServices;

public class AuthorizeService
{
    private readonly DbUserService _dbUserService;
    private readonly JwtService _jwtService;

    public AuthorizeService(DbUserService dbUserService, JwtService jwtService)
    {
        _dbUserService = dbUserService;
        _jwtService = jwtService;
    }

    public async Task<AuthorizeResponse?> Login(AuthorizeRequest request)
    {
        var user = await _dbUserService.AuthenticateUser(request.Login, request.Password);
        return GenerateResponse(user);
    }

    public async Task<AuthorizeResponse?> Auth(string token)
    {
        if (!_jwtService.IsValidToken(token)) return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var userIdClaim =
            jwtToken.Claims.First(claim => claim.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
        var user = await _dbUserService.GetUserByLogin(userIdClaim.Value);
        if (user == null) return null;
        var dto = new DbUserAuthDto(user.Id, user.FirstName, user.SecondName, user.TelephoneNumber, user.Login,
            user.PasswordHash, user.Role);

        return GenerateResponse(dto);
    }

    public async Task<AuthorizeResponse?> Register(RegisterRequest request)
    {
        var user = await _dbUserService.RegisterUser(request.Login, request.Password, request.Role,
            request.Name, request.SecondName, request.Telephone);
        return GenerateResponse(user);
    }

    public async Task<AuthorizeResponse?> ChangePassword(ChangePasswordRequest request)
    {
        var user = await _dbUserService.ChangePassword(request.Login, request.OldPassword, request.NewPassword);
        return GenerateResponse(user);
    }

    public async Task<AuthorizeResponse?> ChangeInfo(ChangeInfoRequest request)
    {
        var user = await _dbUserService.ChangeInfo(request.Login, request.Password, request.Name, request.SecondName,
            request.Telephone, request.Role);
        return GenerateResponse(user);
    }

    private AuthorizeResponse? GenerateResponse(DbUserAuthDto? user)
    {
        if (user == null) return null;
        var dto = user.GetValueOrDefault();
        var token = _jwtService.GenerateToken(dto);
        var name = dto.FirstName + " " + dto.SecondName;
        return new AuthorizeResponse(token, dto.Id, name, dto.Role);
    }
}