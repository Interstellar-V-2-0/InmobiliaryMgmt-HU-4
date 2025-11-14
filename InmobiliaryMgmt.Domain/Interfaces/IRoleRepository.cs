using InmobiliaryMgmt.Domain.Entities;

namespace InmobiliaryMgmt.Domain.Interfaces;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByNameAsync(string name);
}