using InmobiliaryMgmt.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace InmobiliaryMgmt.Application.Interfaces
{
    public interface IPropertyImageService
    {
        Task<IEnumerable<PropertyImage>> GetAllAsync();
        Task<PropertyImage?> GetByIdAsync(int id);
        Task<PropertyImage> CreateAsync(int propertyId, IFormFile file);
        Task<bool> DeleteAsync(int id);
    }
}