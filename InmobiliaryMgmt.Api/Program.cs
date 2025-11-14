using InmobiliaryMgmt.Application.Interfaces;
using InmobiliaryMgmt.Application.Services;
using InmobiliaryMgmt.Domain.Entities;
using InmobiliaryMgmt.Domain.Interfaces;
using InmobiliaryMgmt.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CloudinaryService>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IPropertyImageService, PropertyImageService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "InmobiliaryMgmt API", Version = "v1" });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "InmobiliaryMgmt API v1");
    c.RoutePrefix = string.Empty; // Esto hace que Swagger abra en /
});

app.MapControllers();

app.Run();