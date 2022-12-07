using Domain;

namespace Infrastructure.Interfaces;

/// <summary>
/// A repository interface for users.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Returns a user by username.
    /// </summary>
    User Find(string username);
}