using InmobiliaryMgmt.Domain.Entities;

namespace InmobiliaryMgmt.Domain.Interfaces;

public interface IPropertyRepository : IRepository<Property>
{
    Task<Property?> GetByTitleAsync(string title);
    Task<IEnumerable<Property>> GetAllByUserIdAsync(int userId);
    Task<Property?> GetWithImagesAsync(int id);
    Task<IEnumerable<Property>> GetByRangePriceAsync(decimal min, decimal max);
    Task<IEnumerable<Property>> GetAllWithImagesAsync();
    Task<bool> ExistsByTitleAsync(string title);
}