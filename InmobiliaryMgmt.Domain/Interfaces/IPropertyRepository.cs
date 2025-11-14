using InmobiliaryMgmt.Domain.Entities;

namespace InmobiliaryMgmt.Domain.Interfaces;

public interface IPropertyRepository
{
    Task<Property?> GetByIdAsync(int id);
}