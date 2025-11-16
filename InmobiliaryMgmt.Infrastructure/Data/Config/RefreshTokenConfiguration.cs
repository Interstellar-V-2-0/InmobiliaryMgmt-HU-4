using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InmobiliaryMgmt.Domain.Entities;

namespace InmobiliaryMgmt.Infrastructure.Data.Config
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.Property(rt => rt.IsRevoked)
                .IsRequired()
                .HasDefaultValue(false); 

            
            builder.Property(rt => rt.Token)
                .HasMaxLength(500) 
                .IsRequired();
            
        }
    }
}