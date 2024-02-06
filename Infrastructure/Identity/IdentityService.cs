using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.Basic;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly InspectorGadgetDbContext _context;

    public IdentityService(InspectorGadgetDbContext context)
    {
        _context = context;
    }

    public Task<string?> GetUserRoleAsync(string userId)
    {
        var user = _context.DbUsers.Find(userId);

        return Task.FromResult(user?.Role);
    }

    public Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = _context.DbUsers.Find(userId);

        return Task.FromResult(user?.Role == role);
    }
    
    public async Task<DbUser?> GetUserByLogin(string login)
    {
        return await _context.DbUsers.FirstOrDefaultAsync(x => x.Login == login);
    }
    
    public async Task<(Result, DbUser?)> AuthenticateUser(string login, string password)
    {
        var user = await _context.DbUsers.FirstOrDefaultAsync(x => x.Login == login);
        
        if (user == null)
        {
            var errors = new List<ResultError> { new(ResultErrorEnum.InvalidLogin) };
            return (Result.Failure(errors), null);
        }
        
        if (user.PasswordHash != password)
        {
            var errors = new List<ResultError> { new(ResultErrorEnum.InvalidPassword) };
            return (Result.Failure(errors), null);
        }
        
        return (Result.Success(), user);
    }
}