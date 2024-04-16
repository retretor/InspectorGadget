using System.Text;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Infrastructure.Authorization;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.UseNodaTime();
        var dataSource = dataSourceBuilder.Build();
        services.AddDbContext<InspectorGadgetDbContext>(options => { options.UseNpgsql(dataSource); });

        // services.AddScoped<IApplicationDbContext, InspectorGadgetDbContext>();

        services.AddHttpContextAccessor();
        // Регистрация DbContext
        services.AddScoped<IApplicationDbContext>(serviceProvider =>
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var user = httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }
            Console.WriteLine("Getting new connection for Role: {0}", Utils.GetUserRole(user));

            var dbContextProvider = serviceProvider.GetRequiredService<IAppDbContextProvider>();
            var userRole = Utils.GetUserRole(user);
            var dbContext = dbContextProvider.GetDbContext(userRole);
            if (dbContext == null)
            {
                throw new InvalidDbContextException();
            }

            return dbContext;
        });


        services.AddSingleton<IAppDbContextProvider, AppDbContextProvider>();
        //services.AddScoped<IAppDbContextProvider, AppDbContextProvider>();

        // Add authentication
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.SaveToken = true;
            x.RequireHttpsMetadata = false;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });

        // Add services
        services.AddSingleton<JwtService>();
        services.AddSingleton<IAuthorizationService, AuthorizationService>();
        services.AddSingleton<IIdentityService, IdentityService>();

        return services;
    }
}