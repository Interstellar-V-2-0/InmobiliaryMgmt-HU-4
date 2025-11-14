using InmobiliaryMgmt.Domain.Entities;
using InmobiliaryMgmt.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InmobiliaryMgmt.Infrastructure.Repository;

public class PropertyRepository : IPropertyRepository
{
    private readonly AppDbContext _context;

    public PropertyRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Property?> GetByIdAsync(int id)
    {
        return await _context.Properties
            .Include(P =>P.User)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}