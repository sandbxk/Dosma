using Domain;

namespace Infrastructure.Interfaces;

public interface IUserRepository : IRepository<User>
{
    User Find(string username);
}