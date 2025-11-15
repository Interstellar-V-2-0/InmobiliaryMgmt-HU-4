using InmobiliaryMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InmobiliaryMgmt.Infrastructure.Data.Seed;

public class DbSeeder
{
    public static async Task Seed(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Roles.AnyAsync())
        {
            context.Roles.AddRange(new[]
            {
                new Role { Name = "Admin" },
                new Role { Name = "Client" }
            });
            await context.SaveChangesAsync();
        }

        if (!await context.DocTypes.AnyAsync())
        {
            context.DocTypes.AddRange(new[]
            {
                new DocType { Name = "Citizenship Card" },
                new DocType { Name = "Identity Card" },
                new DocType { Name = "Foreigner ID" },
                new DocType { Name = "Other" }
            });
            await context.SaveChangesAsync();
        }

        if (!await context.Users.AnyAsync(u => u.Email == "SpAdmin@gmail.com"))
        {
            var adminRole = await context.Roles.FirstAsync(r => r.Name == "Admin");
            var firstDocType = await context.DocTypes.FirstAsync();

            context.Users.Add(new User
            {
                Name = "Super Admin",
                LastName = "Boss",
                DocTypeId = firstDocType.Id,
                DocumentNumber = "1234567890",
                RoleId = adminRole.Id,
                Email = "SpAdmin@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("SpAdmin123!"), 
                RegisterDate = DateTime.UtcNow, 
            });
            await context.SaveChangesAsync();
        }
    }
}