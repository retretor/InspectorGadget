using InspectorGadget.Db.ModelRepositories;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services.AuthServices;

public class AuthenticationService
{
    private readonly IDbRepository _repository = new DbUserRepository();

    public async Task<DbUserAuthDto?> AuthenticateUser(string login, string password)
    {
        var users = await _repository.GetAllAsync<DbUser>();
        var user = users?.FirstOrDefault(c => c.Login == login);
        if (user != null && password == user.PasswordHash)
        {
            var dto = new DbUserAuthDto(user.Id, user.FirstName, user.SecondName, user.TelephoneNumber, user.Login, user.PasswordHash,
                user.Role);
            return dto;
        }

        return null;
    }

    public async Task<DbUserAuthDto?> RegisterUser(string login, string password, string role, string name,
        string secondName, string telephone)
    {
        var users = await _repository.GetAllAsync<DbUser>();
        var user = users?.FirstOrDefault(c => c.Login == login);
        if (user != null)
        {
            return null;
        }

        var userRole = role.ToUpper() switch
        {
            "ADMIN" => UserRole.ADMIN,
            "MASTER" => UserRole.MASTER,
            "RECEPTIONIST" => UserRole.RECEPTIONIST,
            _ => UserRole.CLIENT
        };
        var newUser = new DbUserAuthDto(0, name, secondName, telephone, login, password, userRole);
        var dbUserRepository = (DbUserRepository)_repository;
        var createdUser = await dbUserRepository.CreateAsync(newUser);
        if (createdUser == null)
        {
            return null;
        }

        var dto = new DbUserAuthDto(createdUser.Id, createdUser.FirstName, createdUser.SecondName, createdUser.TelephoneNumber, createdUser.Login,
            createdUser.PasswordHash, createdUser.Role);
        return dto;
    }
}