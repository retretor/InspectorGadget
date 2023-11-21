using InspectorGadget.DTOs;

namespace InspectorGadget.Services.AuthServices;

public class AuthorizeService
{
    private readonly AuthenticationService _authenticationService;
    private readonly JwtService _jwtService;

    public AuthorizeService(AuthenticationService authenticationService, JwtService jwtService)
    {
        _authenticationService = authenticationService;
        _jwtService = jwtService;
    }

    public async Task<AuthorizeResponse?> Login(AuthorizeRequest request)
    {
        var user = await _authenticationService.AuthenticateUser(request.Login, request.Password);
        if (user == null)
        {
            return null;
        }

        var dto = user.GetValueOrDefault();
        var token = _jwtService.GenerateToken(dto);
        return new AuthorizeResponse
        {
            Token = token,
            ClientId = dto.Id,
            ClientName = dto.FirstName + " " + dto.SecondName,
            ClientType = dto.Role.ToString()
        };
    }

    public async Task<AuthorizeResponse?> Register(RegisterRequest request)
    {
        var user = await _authenticationService.RegisterUser(request.Login, request.Password, request.Role,
            request.Name, request.SecondName, request.Telephone);
        if (user == null)
        {
            return null;
        }

        var dto = user.GetValueOrDefault();
        var token = _jwtService.GenerateToken(dto);
        return new AuthorizeResponse
        {
            Token = token,
            ClientId = dto.Id,
            ClientName = dto.FirstName + " " + dto.SecondName,
            ClientType = dto.Role.ToString()
        };
    }
}