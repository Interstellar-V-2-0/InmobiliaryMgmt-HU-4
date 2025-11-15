using InmobiliaryMgmt.Domain.Entities;
using InmobiliaryMgmt.Domain.Interfaces;
using InmobiliaryMgmt.Infrastructure.Data;
using InmobiliaryMgmt.Infrastructure.Repositories;

namespace InmobiliaryMgmt.Infrastructure.Repositories;

public class ContactRequestRepository : Repository<ContactRequest>, IContactRequestRepository
{
    public ContactRequestRepository(AppDbContext context) : base(context)
    {
    }
    
}