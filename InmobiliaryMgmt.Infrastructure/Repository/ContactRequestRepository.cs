using InmobiliaryMgmt.Domain.Entities;
using InmobiliaryMgmt.Domain.Interfaces;

namespace InmobiliaryMgmt.Infrastructure.Repository;

public class ContactRequestRepository : IContactRequestRepository
{
    private readonly AppDbContext _context;

    public ContactRequestRepository(AppDbContext context)
    {
        _context = context;
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