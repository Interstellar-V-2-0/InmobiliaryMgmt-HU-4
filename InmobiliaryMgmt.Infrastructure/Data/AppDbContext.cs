using Microsoft.EntityFrameworkCore;
using InmobiliaryMgmt.Domain.Entities;
using System.Reflection.Emit;

namespace InmobiliaryMgmt.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<ContactRequest> ContactRequests { get; set; }
        public DbSet<DocType> DocTypes { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                
                entity.Property(u => u.Name).IsRequired().HasMaxLength(50);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(120);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.DocumentNumber).HasMaxLength(50);

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
            
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(50);
            });
            
            modelBuilder.Entity<DocType>(entity =>
            {
                entity.HasKey(dt => dt.Id);
                entity.Property(dt => dt.Name).IsRequired().HasMaxLength(50);
            });
            
            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Title).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Description).HasMaxLength(1000);
                entity.Property(p => p.Address).HasMaxLength(200);
                entity.Property(p => p.Price).HasPrecision(18,2); 
                
                entity.HasIndex(p => p.Title).IsUnique(); 

                entity.HasOne(p => p.User)
                      .WithMany(u => u.Properties)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(p => p.PropertyImages)
                      .WithOne(pi => pi.Property)
                      .HasForeignKey(pi => pi.PropertyId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.ContactRequests)
                      .WithOne(cr => cr.Property)
                      .HasForeignKey(cr => cr.PropertyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            
            modelBuilder.Entity<PropertyImage>(entity =>
            {
                entity.HasKey(pi => pi.Id);

                entity.Property(pi => pi.Url).IsRequired().HasMaxLength(500);
                entity.Property(pi => pi.PublicId).IsRequired().HasMaxLength(200); 
            });
            
            modelBuilder.Entity<ContactRequest>(entity =>
            {
                entity.HasKey(cr => cr.Id);

                entity.Property(cr => cr.Message).IsRequired().HasMaxLength(1000); 
                entity.Property(cr => cr.SentDate).IsRequired();

                entity.HasOne(cr => cr.User)
                      .WithMany(u => u.ContactRequests)
                      .HasForeignKey(cr => cr.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(cr => cr.Property)
                      .WithMany(p => p.ContactRequests)
                      .HasForeignKey(cr => cr.PropertyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Token); 
                
                entity.Property(rt => rt.Token).IsRequired().HasMaxLength(500); 
                
                entity.Property(rt => rt.ExpiryDate).IsRequired();
                entity.Property(rt => rt.IsRevoked).HasDefaultValue(false); 

                entity.HasOne(rt => rt.User)
                      .WithMany() 
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}