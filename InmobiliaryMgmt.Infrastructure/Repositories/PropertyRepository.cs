using InmobiliaryMgmt.Domain.Entities;
using InmobiliaryMgmt.Domain.Interfaces;
using InmobiliaryMgmt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InmobiliaryMgmt.Infrastructure.Repositories;

public class PropertyRepository : Repository<Property>, IPropertyRepository
{
    public PropertyRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Property?> GetByNameAsync(string name)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Title == name);
    }

    public async Task<IEnumerable<Property>> GetAllByUserIdAsync(int userId)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> GetByRangePrice(double min, double max)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => p.Price >= min && p.Price <= max)
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> GetAllWithImagesAsync()
    {
        return await _dbSet
            .Include(p => p.PropertyImages)
            .ToListAsync();
    }

    public async Task<Property?> GetWithImagesAsync(int id)
    {
        return await _dbSet
            .Include(p => p.PropertyImages)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    
    public async Task<bool> ExistsByNameAsync(string name)
    {
        return  await _dbSet
            .AnyAsync(p => p.Title == name);
    }
}