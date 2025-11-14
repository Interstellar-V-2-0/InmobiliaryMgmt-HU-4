using Microsoft.EntityFrameworkCore;
using InmobiliaryMgmt.Domain.Entities;

namespace InmobiliaryMgmt.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        // Tablas existentes
        // public DbSet<Property> Properties { get; set; }

        // NUESTRA NUEVA TABLA
        public DbSet<PropertyImage> PropertyImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de PropertyImage
            modelBuilder.Entity<PropertyImage>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(500); // opcional, limita la longitud de la URL

                // Relación con Property
                entity.HasOne(e => e.Property)
                    .WithMany(p => p.PropertyImages) // Asegúrate que Property tenga ICollection<PropertyImage> Images
                    .HasForeignKey(e => e.PropertyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}