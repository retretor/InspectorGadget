using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.Basic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly IApplicationDbContext _context;

    public IdentityService(IAppDbContextProvider contextProvider)
    {
        var context = contextProvider.GetDbContext(Role.ANONYMOUS);
        _context = context ?? throw new UnableToConnectToDatabaseException();
    }

    public async Task<DbUser?> GetUserByLogin(string login)
    {
        return await _context.DbUsers.FirstOrDefaultAsync(x => x.Login == login);
    }

    public async Task<(Result, DbUser?)> GetUserByTelephone(string telephoneNumber, string secretKey)
    {
        var userLogin = await Task.Run(() =>
            _context.GetUserLoginByTelephone(telephoneNumber, secretKey).SingleOrDefaultAsync());
        if (userLogin == null || userLogin.Result == "")
        {
            return (Result.Failure(new NotFoundException(nameof(DbUser), telephoneNumber)), null);
        }

        return (Result.Success(), await _context.DbUsers.FirstOrDefaultAsync(x => x.Login == userLogin.Result));
    }

    public async Task<(Result, DbUser?)> AuthenticateUser(string login, string password)
    {
        var userState = await Task.Run(() => _context.AuthenticateUser(login, password).SingleOrDefaultAsync());
        if (userState == null)
        {
            var errors = new List<ResultError> { new(ResultErrorEnum.Unknown) };
            return (Result.Failure(errors), null);
        }

        switch (userState.Result)
        {
            case 1:
            {
                var errors = new List<ResultError> { new(ResultErrorEnum.InvalidLogin) };
                return (Result.Failure(errors), null);
            }
            case 2:
            {
                var errors = new List<ResultError> { new(ResultErrorEnum.InvalidPassword) };
                return (Result.Failure(errors), null);
            }
            default:
            {
                var user = await _context.DbUsers.FirstOrDefaultAsync(x => x.Login == login);
                return (Result.Success(), user);
            }
        }
    }

    public async Task<(Result result, DbUser? dbUser)> ChangePassword(string login, string oldPasswordHash,
        string newPasswordHash)
    {
        var changeResult = await Task.Run(() =>
            _context.ChangePassword(login, oldPasswordHash, newPasswordHash).SingleOrDefaultAsync());
        if (changeResult == null)
        {
            var errors = new List<ResultError> { new(ResultErrorEnum.Unknown) };
            return (Result.Failure(errors), null);
        }

        switch (changeResult.Result)
        {
            case 1:
            {
                var errors = new List<ResultError> { new(ResultErrorEnum.InvalidLogin) };
                return (Result.Failure(errors), null);
            }
            case 2:
            {
                var errors = new List<ResultError> { new(ResultErrorEnum.InvalidPassword) };
                return (Result.Failure(errors), null);
            }
            default:
            {
                return (Result.Success(), await _context.DbUsers.FirstOrDefaultAsync(x => x.Login == login));
            }
        }
    }
}