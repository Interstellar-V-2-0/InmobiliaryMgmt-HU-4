using InmobiliaryMgmt.Application.Interfaces;
using InmobiliaryMgmt.Application.Services;
using InmobiliaryMgmt.Domain.Interfaces;
using InmobiliaryMgmt.Infrastructure.Data;
using InmobiliaryMgmt.Infrastructure.Repositories;
using InmobiliaryMgmt.Infrastructure.Email;
using InmobiliaryMgmt.Api.Swagger;
using InmobiliaryMgmt.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// =========================================
//           CONFIGURACIÓN GENERAL
// =========================================

// Cloudinary
builder.Services.AddSingleton<CloudinaryService>();

// DbContext con MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 33))
    )
);

// =========================================
//           REPOSITORIOS
// =========================================
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
builder.Services.AddScoped<IContactRequestRepository, ContactRequestRepository>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();

// =========================================
//             SERVICES
// =========================================
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPropertyImageService, PropertyImageService>();
builder.Services.AddScoped<IContactRequestService, ContactRequestService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// =========================================
//              SWAGGER
// =========================================
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "InmobiliaryMgmt API",
        Version = "v1"
    });

    // Para subir archivos desde Swagger
    c.OperationFilter<SwaggerFileOperationFilter>();
});

var app = builder.Build();

// =========================================
//              MIDDLEWARE
// =========================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "InmobiliaryMgmt API v1");
        c.RoutePrefix = string.Empty; // Swagger abre en "/"
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();