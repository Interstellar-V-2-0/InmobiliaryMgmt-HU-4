using InmobiliaryMgmt.Domain.Entities;

namespace InmobiliaryMgmt.Domain.Interfaces;

public interface IContactRequestRepository
{
    Task AddAsync(ContactRequest request);
    Task SaveChangesAsync();
}