using InmobiliaryMgmt.Domain.Entities;

namespace InmobiliaryMgmt.Domain.Interfaces;

public interface IPropertyRepository : IRepository<Property>
{
    Task<Property?> GetByNameAsync(string name);
    Task<IEnumerable<Property>> GetAllByUserIdAsync(int userId);
    Task<Property?> GetWithImagesAsync(int id);
    Task<IEnumerable<Property>> GetByRangePrice(double min, double max);
    Task<IEnumerable<Property>> GetAllWithImagesAsync();
    Task<bool> ExistsByNameAsync(string name);
}