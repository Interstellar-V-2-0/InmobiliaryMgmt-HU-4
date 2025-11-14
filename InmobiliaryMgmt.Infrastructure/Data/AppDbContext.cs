using InmobiliaryMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InmobiliaryMgmt.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // ENTIDADES / TABLAS
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<DocType> DocTypes { get; set; }
        public DbSet<ContactRequest> ContactRequests { get; set; }

        // CONFIGURACIÓN DEL MODELO
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Name).IsRequired().HasMaxLength(50);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(120);

                entity.HasIndex(u => u.Email).IsUnique();

                entity.HasOne(u => u.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(u => u.RoleId);

                entity.HasOne(u => u.DocType)
                      .WithMany(dt => dt.Users)
                      .HasForeignKey(u => u.DocTypeId);
            });

            // Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(50);
            });

            // DocType
            modelBuilder.Entity<DocType>(entity =>
            {
                entity.HasKey(dt => dt.Id);
                entity.Property(dt => dt.Name).IsRequired().HasMaxLength(50);
            });

            // Property
            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Title)
                    .IsRequired()
                    .HasMaxLength(100);
                
            });

            // ContactRequest
            modelBuilder.Entity<ContactRequest>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Message)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.HasOne(c => c.User)
                      .WithMany(u => u.ContactRequests)
                      .HasForeignKey(c => c.UserId);

                entity.HasOne(c => c.Property)
                      .WithMany()
                      .HasForeignKey(c => c.PropertyId);
            });

            // SEED ROLES (Administrador, Cliente)
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Administrador" },
                new Role { Id = 2, Name = "Cliente" }
            );
        }
    }
}
