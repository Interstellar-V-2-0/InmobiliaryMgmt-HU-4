using InmobiliaryMgmt.Domain.Entities;
using InmobiliaryMgmt.Domain.Interfaces;
using InmobiliaryMgmt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InmobiliaryMgmt.Infrastructure.Repositories;

public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByTokenWithUserAsync(string token)
    {
        return await _dbSet
            .Include(rt => rt.User!)
            .ThenInclude(u => u.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked);
    }
}