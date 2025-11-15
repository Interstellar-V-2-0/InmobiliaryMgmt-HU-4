using Microsoft.EntityFrameworkCore;
using InmobiliaryMgmt.Domain.Entities;

namespace InmobiliaryMgmt.Infrastructure.Data
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
        public DbSet<PropertyImage> PropertyImages { get; set; }

        // CONFIGURACIÓN DEL MODELO
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ================================
            // USER
            // ================================
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

            // ================================
            // ROLE
            // ================================
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(50);
            });

            // ================================
            // DOCTYPE
            // ================================
            modelBuilder.Entity<DocType>(entity =>
            {
                entity.HasKey(dt => dt.Id);
                entity.Property(dt => dt.Name).IsRequired().HasMaxLength(50);
            });

            // ================================
            // PROPERTY
            // ================================
            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Title)
                      .IsRequired()
                      .HasMaxLength(100);

                // Relación con imágenes
                entity.HasMany(p => p.PropertyImages)
                      .WithOne(pi => pi.Property)
                      .HasForeignKey(pi => pi.PropertyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ================================
            // PROPERTY IMAGE
            // ================================
            modelBuilder.Entity<PropertyImage>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Url)
                      .IsRequired()
                      .HasMaxLength(500);
            });

            // ================================
            // CONTACT REQUEST
            // ================================
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
        }
    }
}
