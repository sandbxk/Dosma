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

    public List<User> All()
    {
        return _DBContext.UserTable.ToList();
    }

    public User Create(User user)
    {
        _DBContext.UserTable.Add(user);
        _DBContext.SaveChanges();
        return user;
    }

    public User Delete(int id)
    {
        User? user = _DBContext.UserTable.Find(id);
        if (user != null)
        {
            _DBContext.UserTable.Remove(user);
            _DBContext.SaveChanges();
            return user;
        }

        throw new NullReferenceException("User not found");
    }

    public User Single(long id)
    {
        return _DBContext.UserTable.Find(id) ?? throw new NullReferenceException("User not found");
    }

    public User Find(string username)
    {
        return _DBContext.UserTable.Where(u => u.Username == username).FirstOrDefault() ?? throw new NullReferenceException("User not found");
    }

    public User Update(long id, User model)
    {
        User? user = _DBContext.UserTable.Find(id);
        if (user != null)
        {
            user.Username = model.Username;
            user.HashedPassword = model.HashedPassword;
            user.DisplayName = model.DisplayName;
            user.GroceryLists = model.GroceryLists;
            _DBContext.SaveChanges();
            return user;
        }

        throw new NullReferenceException("User not found");
    }
}