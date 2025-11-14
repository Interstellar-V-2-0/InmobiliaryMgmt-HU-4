using InmobiliaryMgmt.Domain.Entities;
using InmobiliaryMgmt.Domain.Interfaces;
using InmobiliaryMgmt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InmobiliaryMgmt.Infrastructure.Repositories
{
    public class PropertyImageRepository : Repository<PropertyImage>, IPropertyImageRepository
    {
        public PropertyImageRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<PropertyImage>> GetByPropertyIdAsync(int propertyId)
        {
            return await _context.PropertyImages
                .Where(pi => pi.PropertyId == propertyId)
                .ToListAsync();
        }
    }
}