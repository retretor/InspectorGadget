using Application.Common.Interfaces;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Data;

public class AppDbContextProvider : IAppDbContextProvider
{
    private readonly IConfiguration _configuration;

    public AppDbContextProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public IApplicationDbContext? GetDbContext(Role role)
    {
        Console.WriteLine($"Creating new connection for Role: {role}");
        var connectionString = GetConnectionString(role);
        if (connectionString == null) return null;

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

        dataSourceBuilder.UseNodaTime();
        var dataSource = dataSourceBuilder.Build();
        var options = new DbContextOptionsBuilder<InspectorGadgetDbContext>().UseNpgsql(dataSource).Options;
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