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

        // Tablas / Entidades
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<DocType> DocTypes { get; set; }
        public DbSet<ContactRequest> ContactRequests { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------------- User ----------------
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Name).IsRequired().HasMaxLength(50);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(120);
                entity.HasIndex(u => u.Email).IsUnique();

                entity.HasOne(u => u.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(u => u.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.DocType)
                      .WithMany(dt => dt.Users)
                      .HasForeignKey(u => u.DocTypeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------------- Role ----------------
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(50);
            });

            // ---------------- DocType ----------------
            modelBuilder.Entity<DocType>(entity =>
            {
                entity.HasKey(dt => dt.Id);
                entity.Property(dt => dt.Name).IsRequired().HasMaxLength(50);
            });

            // ---------------- Property ----------------
            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Title).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Description).IsRequired();
                entity.Property(p => p.Address).IsRequired();
                entity.Property(p => p.Price).IsRequired();

                entity.HasMany(p => p.PropertyImages)
                      .WithOne(pi => pi.Property)
                      .HasForeignKey(pi => pi.PropertyId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.ContactRequests)
                      .WithOne(c => c.Property)
                      .HasForeignKey(c => c.PropertyId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.User)
                      .WithMany(u => u.Properties)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------------- PropertyImage ----------------
            modelBuilder.Entity<PropertyImage>(entity =>
            {
                entity.HasKey(pi => pi.Id);
                entity.Property(pi => pi.Url).IsRequired().HasMaxLength(500);
                entity.Property(pi => pi.PublicId).IsRequired().HasMaxLength(500);
            });

            // ---------------- ContactRequest ----------------
            modelBuilder.Entity<ContactRequest>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Message).IsRequired().HasMaxLength(500);
                entity.Property(c => c.SentDate).IsRequired();

                entity.HasOne(c => c.User)
                      .WithMany(u => u.ContactRequests)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Property)
                      .WithMany(p => p.ContactRequests)
                      .HasForeignKey(c => c.PropertyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}

