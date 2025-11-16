using InmobiliaryMgmt.Domain.Entities;

namespace InmobiliaryMgmt.Domain.Interfaces;

public interface IDocTypeRepository : IRepository<DocType>
{
 Task<DocType?> GetByNameAsync(string name);
}