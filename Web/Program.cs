using Application;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Web;
using Web.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices(builder.Configuration);

// Add swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inspector Gadgets API", Version = "v1" });
});
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

// Add cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsPolicyBuilder =>
        corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

    options.AddPolicy("AllowSpecificOrigin",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
            corsPolicyBuilder
                .WithOrigins("https://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Build the app
var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(builder.Configuration.GetValue<string>("ImageSettings:ImagePath")!),
    RequestPath = "/images"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inspector Gadgets API V1"); });
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

/*TODO:
frontend:
проверить на стороне пользователя logout функцию!!!!!
кешировать данные на стороне пользователя
проверить все validateField
пересмотреть все роуты
проверить что при обновлении страницы у клиента запрос делается дважды в любых случаях!!!!!
поиск неправильно работает в фильтрах!!!!!
в фильтрах не пишется количество эллементов!!!!!
сделать отдельный класс для поиска и там реализовать продвинутый поиск по всем полям!!!!!
пересмотреть все sort bars
пересмотреть все const sort = () => {}
доделать repairRequest (добавить внутрь сущности клиента, работника и устройства)
изменить deviceId в repairRequest на can be null и это должен будет назначать работник ресепшена
использовать needToReload во всех сущностях
добавить подсказки при неправильном пароле или логине




backend:
Change permissions on sql server side
переписать привелегии у всех включа админа
во всех сущностях из бд прописать [Column(<column_name>)]
пересмотреть классы в директориях ./Entities: Composite и Responses
кешировать данные на стороне сервера
 */