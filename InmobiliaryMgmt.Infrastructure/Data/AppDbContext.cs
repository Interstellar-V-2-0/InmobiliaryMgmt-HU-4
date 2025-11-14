using InmobiliaryMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InmobiliaryMgmt.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<DocType> DocTypes { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<PropertyImage> PropertyImages { get; set; }
    public DbSet<ContactRequest> ContactRequests { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder){}
}