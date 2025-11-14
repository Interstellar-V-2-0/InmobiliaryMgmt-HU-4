using InmobiliaryMgmt.Domain.Entities;

namespace InmobiliaryMgmt.Domain.Interfaces
{
    public interface IPropertyImageRepository : IRepository<PropertyImage>
    {
        Task<IEnumerable<PropertyImage>> GetByPropertyIdAsync(int propertyId);
    }
}