﻿using Application.Common.Models;
using Domain.Entities.Basic;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserRoleAsync(string userId);
    Task<bool> IsInRoleAsync(string userId, string role);
    Task<DbUser?> GetUserByLogin(string login);
    Task<(Result, DbUser?)> AuthenticateUser(string login, string password);
}