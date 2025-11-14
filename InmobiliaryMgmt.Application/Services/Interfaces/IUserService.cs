namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<string> Register(string name, string lastName, string email, string password, int roleId, int docTypeId);
        Task<string?> Login(string email, string password);
    }
}