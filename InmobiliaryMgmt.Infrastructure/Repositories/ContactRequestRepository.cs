using InmobiliaryMgmt.Domain.Entities;
using InmobiliaryMgmt.Domain.Interfaces;
using InmobiliaryMgmt.Infrastructure.Data;
using InmobiliaryMgmt.Infrastructure.Repositories;

namespace InmobiliaryMgmt.Infrastructure.Repository;

public class ContactRequestRepository : Repository<ContactRequest>, IContactRequestRepository
{
    public ContactRequestRepository(AppDbContext context) : base(context)
    {
    }

    public async Task AddAsync(ContactRequest request)
    {
        await _context.ContactRequests.AddAsync(request);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    
}