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
    
    public async Task<Property?> GetByTitleAsync(string title)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Title == title);
    }
    
    public async Task<IEnumerable<Property>> GetAllByUserIdAsync(int userId)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Property>> GetByRangePriceAsync(decimal min, decimal max)
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
    
    public async Task<bool> ExistsByTitleAsync(string title)
    {
        return  await _dbSet
            .AnyAsync(p => p.Title == title);
    }

    public async Task<Property?> GetWithImagesAsync(int id)
    {
        return await _dbSet
            .Include(p => p.PropertyImages)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public override async Task<Property?> GetByIdAsync(int id)
    {
        return await _context.Properties
            .Include(p => p.User)
            .Include(p => p.PropertyImages)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}