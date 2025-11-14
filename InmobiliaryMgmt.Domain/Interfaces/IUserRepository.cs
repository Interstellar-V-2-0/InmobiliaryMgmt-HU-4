using InmobiliaryMgmt.Domain.Entities;
using InmobiliaryMgmt.Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmail(string email);
        Task Add(User user);
        Task<bool> EmailExists(string email);
    }
}