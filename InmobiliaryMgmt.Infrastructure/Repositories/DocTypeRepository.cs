using InmobiliaryMgmt.Domain.Entities;
using InmobiliaryMgmt.Domain.Interfaces;
using InmobiliaryMgmt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InmobiliaryMgmt.Infrastructure.Repositories;

public class DocTypeRepository : Repository<DocType>, IDocTypeRepository
{
    public DocTypeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<DocType?> GetByNameAsync(string name)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Name == name);
    }
}