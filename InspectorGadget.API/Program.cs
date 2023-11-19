using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InspectorGadget.Db;
using InspectorGadget.Identity;
using InspectorGadget.Services;
using InspectorGadget.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(IdentityData.AdminUserPolicy, p =>
        p.RequireClaim(IdentityData.AdminUserClaim, "true"));
});

builder.Services.AddControllers();

// Scopes
builder.Services.AddScoped<DeviceService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<AuthorizeService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

// Add db
builder.Services.AddDbContext<InspectorGadgetContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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

// app.Map("/login/{username}", (string username) =>
// {
//     var claims = new List<Claim>
//     {
//         new(ClaimTypes.Name, username),
//         new(IdentityData.AdminUserClaim, "true")
//     };
//     
//     var jwt = new JwtSecurityToken(
//         issuer: builder.Configuration["JwtSettings:Issuer"],
//         audience: builder.Configuration["JwtSettings:Audience"],
//         claims: claims,
//         expires: DateTime.UtcNow.Add(TimeSpan.FromHours(8)),
//         signingCredentials: new SigningCredentials(
//             new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
//             SecurityAlgorithms.HmacSha256)
//     );
//
//     Console.WriteLine($"Generated JWT for {username} with claims: {string.Join(", ", claims)} is {jwt}");
//     return new JwtSecurityTokenHandler().WriteToken(jwt);
// });
//
//
// app.Map("/data", [Authorize](context) => context.Response.WriteAsync("Hello World!"));

app.MapControllers();
app.Run();
