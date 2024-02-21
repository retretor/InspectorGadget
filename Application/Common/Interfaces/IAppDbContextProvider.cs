using Domain.Enums;

namespace Application.Common.Interfaces;

public interface IAppDbContextProvider
{
    IApplicationDbContext? GetDbContext(Role role);
}