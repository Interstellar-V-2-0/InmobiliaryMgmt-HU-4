using InmobiliaryMgmt.Api.Swagger;
using InmobiliaryMgmt.Application.Interfaces;
using InmobiliaryMgmt.Application.Services;
using InmobiliaryMgmt.Domain.Interfaces;
using InmobiliaryMgmt.Infrastructure.Data;
using InmobiliaryMgmt.Infrastructure.Data.Seed;
using InmobiliaryMgmt.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using DotNetEnv;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// CARGAR VARIABLES DE ENTORNO
// ==========================================
Env.Load();

// ==========================================
// 1. CONFIGURAR CADENA DE CONEXIÓN
// ==========================================
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
if (string.IsNullOrEmpty(connectionString))
    throw new InvalidOperationException("La variable de entorno 'DB_CONNECTION' no está definida.");

// ==========================================
// 2. REGISTRAR AppDbContext
// ==========================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// ==========================================
// 3. REGISTRAR REPOSITORIOS
// ==========================================
// Repositorios concretos
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
builder.Services.AddScoped<IContactRequestRepository, ContactRequestRepository>();
builder.Services.AddScoped<IDocTypeRepository, DocTypeRepository>();
// AÑADIDO: Registro del repositorio de Refresh Token
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>(); 

// Registrar repositorios genéricos para todos los servicios que dependan de IRepository<T>
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// ==========================================
// 4. REGISTRAR SERVICIOS
// ==========================================
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPropertyImageService, PropertyImageService>();
builder.Services.AddScoped<IContactRequestService, ContactRequestService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Cloudinary Service (Singleton)
builder.Services.AddSingleton<CloudinaryService>();

// ==========================================
// 5. JWT AUTHENTICATION
// ==========================================
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
{
    throw new InvalidOperationException("Las variables de entorno JWT_KEY, JWT_ISSUER y JWT_AUDIENCE deben estar definidas.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),
        ClockSkew = TimeSpan.Zero
    };
});

// ==========================================
// 6. SWAGGER (OPENAPI)
// ==========================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // AÑADIDO: Registro del filtro para manejar IFormFile en Swagger
    options.OperationFilter<SwaggerFileOperationFilter>();

    // AÑADIDO: Configuración de seguridad JWT en Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT con el prefijo Bearer. Ejemplo: 'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// ==========================================
// 7. CORS
// ==========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ==========================================
// 8. CONTROLLERS
// ==========================================
builder.Services.AddControllers();

var app = builder.Build();

// ==========================================
// 9. AUTO-APLICAR MIGRACIONES Y SEEDER
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Usar 'EnsureCreated' solo en desarrollo. 'MigrateAsync' en producción.
    await db.Database.MigrateAsync(); 
    await DbSeeder.Seed(db);
}

// ==========================================
// 10. MIDDLEWARES
// ==========================================
app.UseCors("AllowAll");

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