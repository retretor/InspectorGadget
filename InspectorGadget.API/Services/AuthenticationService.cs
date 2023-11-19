using System.Security.Cryptography;
using System.Text;
using InspectorGadget.Db;
using InspectorGadget.DTOs;
using InspectorGadget.Models;

namespace InspectorGadget.Services;

public class AuthenticationService
{
    private readonly DbRepository _repository = new();
    public async Task<DbUserAuthDto?> AuthenticateUser(string login, string password)
    {
        var users = await _repository.GetAllAsync<DbUser>();
        var user = users?.FirstOrDefault(c => c.Login == login);
        if (user != null && VerifyPasswordHash(password, user.PasswordHash))
        {
            var dto = new DbUserAuthDto(user.Id, user.FirstName, user.SecondName, user.Login, user.PasswordHash,
                user.Role);
            return dto;
        }

        return null;
    }

    // public async Task<Client?> AuthenticateClient(string login, string password)
    // {
    //     var user = await _context.DbUsers.FirstOrDefaultAsync(c => c.Login == login);
    //     if (user != null && VerifyPasswordHash(password, user.PasswordHash))
    //     {
    //         var client = await _context.Clients.FirstOrDefaultAsync(c => c.DbUser == user);
    //         return client;
    //     }
    //     return null;
    // }
    //
    // public async Task<Employee?> AuthenticateEmployee(string login, string password)
    // {
    //     var user = await _context.DbUsers.FirstOrDefaultAsync(c => c.Login == login);
    //     if (user != null && VerifyPasswordHash(password, user.PasswordHash))
    //     {
    //         var employee = await _context.Employees.FirstOrDefaultAsync(c => c.DbUser == user);
    //         return employee;
    //     }
    //     return null;
    // }

    // TODO: move to Frontend
    private bool VerifyPasswordHash(string password, string storedHash)
    {
        // TODO: delete this line
        return password == storedHash;
        
        using var hmac = new HMACSHA512();
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        Console.WriteLine("Computed hash: " + Convert.ToBase64String(computedHash));
        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != storedHash[i])
            {
                return false;
            }
        }

        return true;
    }
}