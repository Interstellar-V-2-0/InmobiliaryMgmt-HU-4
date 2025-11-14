using InmobiliaryMgmt.Domain.Entities;

namespace Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);
        Task Add(User user);
        Task<bool> EmailExists(string email);
    }
}