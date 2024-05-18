using Application.Common.Interfaces;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Data;

public class AppDbContextProvider : IAppDbContextProvider
{
    private readonly IConfiguration _configuration;
    private static readonly Dictionary<Role, DbContextOptions<InspectorGadgetDbContext>> OptionsCache = new();

    public AppDbContextProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public IApplicationDbContext? GetDbContext(Role role)
    {
        var connectionString = GetConnectionString(role);
        if (connectionString == null) return null;

        if (OptionsCache.TryGetValue(role, out var options))
        {
            return new InspectorGadgetDbContext(options);
        }

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.UseNodaTime();
        var dataSource = dataSourceBuilder.Build();
        options = new DbContextOptionsBuilder<InspectorGadgetDbContext>().UseNpgsql(dataSource).Options;
        OptionsCache.Add(role, options);
        var dbContext = new InspectorGadgetDbContext(options);

        return dbContext;
    }

    private string? GetConnectionString(Role role)
    {
        var connectionStringName = role switch
        {
            Role.CLIENT => "ClientConnection",
            Role.ADMIN => "AdminConnection",
            Role.RECEPTIONIST => "ReceptionistConnection",
            Role.MASTER => "MasterConnection",
            Role.ANONYMOUS => "AnonymousConnection",
            _ => null
        };
        if (connectionStringName == null) return null;

        var connectionString = _configuration.GetConnectionString(connectionStringName);
        return connectionString;
    }
}