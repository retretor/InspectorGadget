using System.Text;
using InspectorGadget.Db;
using InspectorGadget.Models;
using InspectorGadget.Services.AuthServices;
using InspectorGadget.Services.ModelServices;
using InspectorGadget.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(x =>
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
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddControllers();

// Scoped
builder.Services.AddScoped<AllowedRepairTypesForEmployeeService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<DeviceService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<PartForRepairPartService>();
builder.Services.AddScoped<RepairPartService>();
builder.Services.AddScoped<RepairRequestService>();
builder.Services.AddScoped<RepairTypeForDeviceService>();
builder.Services.AddScoped<RepairTypeService>();
builder.Services.AddScoped<RepairTypesListService>();
builder.Services.AddScoped<RequestStatusHistoryService>();


builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<AuthorizeService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();


// Add db
// Создание объекта NpgsqlDataSourceBuilder
var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
dataSourceBuilder.UseNodaTime()
    .MapEnum<RepairPartCondition>()
    .MapEnum<RequestStatus>()
    .MapEnum<UserRole>();
var dataSource = dataSourceBuilder.Build(); // Построение источника данных

// Настройка контекста для использования источника данных
builder.Services.AddDbContext<InspectorGadgetContext>(options => { options.UseNpgsql(dataSource); });

// builder.Services.AddDbContext<InspectorGadgetContext>(options =>
// {
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();