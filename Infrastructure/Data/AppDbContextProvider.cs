using Application.Common.Interfaces;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Data;

public class AppDbContextProvider : IAppDbContextProvider
{
    private readonly Dictionary<Role, IApplicationDbContext> _contexts = new();
    private readonly IConfiguration _configuration;

    public AppDbContextProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public IApplicationDbContext? GetDbContext(Role role)
    {
        if (_contexts.TryGetValue(role, out var context)) return context;

        var connectionString = GetConnectionString(role);
        if (connectionString == null) return null;

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

        dataSourceBuilder.UseNodaTime();
        var dataSource = dataSourceBuilder.Build();
        var options = new DbContextOptionsBuilder<InspectorGadgetDbContext>().UseNpgsql(dataSource).Options;
        var dbContext = new InspectorGadgetDbContext(options);

        _contexts.Add(role, dbContext);

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