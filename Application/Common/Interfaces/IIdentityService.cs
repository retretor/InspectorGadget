using Application.Common.Models;
using Domain.Entities.Basic;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<DbUser?> GetUserByLogin(string login);
    Task<(Result, DbUser?)> AuthenticateUser(string login, string password);
    Task<(Result result, DbUser? dbUser)> ChangePassword(string login, string oldPasswordHash, string newPasswordHash);
    Task<(Result result, DbUser? dbUser)> GetUserByTelephone(string telephoneNumber, string secretKey);
}