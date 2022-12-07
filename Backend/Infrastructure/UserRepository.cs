using Backend.Helpers;
using Domain;
using Infrastructure.Interfaces;

namespace Infrastructure;

public class UserRepository : IUserRepository
{
    private DatabaseContext _DBContext;
    
    public UserRepository(DatabaseContext DBContext)
    {
        _DBContext = DBContext;
    }

    /// <inheritdoc />
    public List<User> All()
    {
        return _DBContext.UserTable.ToList();
    }

    /// <inheritdoc />
    public User Create(User user)
    {
        _DBContext.UserTable.Add(user);
        _DBContext.SaveChanges();
        return user;
    }

    /// <inheritdoc />
    public bool Delete(int id)
    {
        User user = _DBContext.UserTable.Find(id) ?? throw new NullReferenceException("User not found");

        _DBContext.UserTable.Remove(user);
        int change = _DBContext.SaveChanges();
        
        if (change <= 0)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc />
    public User Single(long id)
    {
        return _DBContext.UserTable.Find(id) ?? throw new NullReferenceException("User does not exist");
    }

    /// <inheritdoc />
    public User Find(string username)
    {
        return _DBContext.UserTable.Where(u => u.Username == username).FirstOrDefault() ?? throw new NullReferenceException("User not found");
    }

    /// <inheritdoc />
    public User Update(long id, User model)
    {
        User user = _DBContext.UserTable.Find(id) ?? throw new NullReferenceException("User not found");
        
        user.Username = model.Username;
        user.HashedPassword = model.HashedPassword;
        user.DisplayName = model.DisplayName;
        user.GroceryLists = model.GroceryLists;
        _DBContext.SaveChanges();
        return user;
    }

    /// <inheritdoc />
    public User Update(User model)
    {
        return this.Update(model.Id, model);
    }
}