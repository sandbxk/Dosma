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
        var ul = _DBContext.Users.ToList();

        foreach (var user in ul)
        {
            user.GroceryLists = _DBContext.UserGroceryLists.Where(u => u.UserID == user.Id).Select(u => u.GroceryList).ToList();
        }

        return ul;
    }

    /// <inheritdoc />
    public User Create(User user)
    {
        _DBContext.Users.Add(user);
        _DBContext.SaveChanges();
        return user;
    }

    /// <inheritdoc />
    public bool Delete(int id)
    {
        User user = _DBContext.Users.Find(id) ?? throw new NullReferenceException("User not found");

        _DBContext.Users.Remove(user);
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
        var user = _DBContext.Users.Find(id) ?? throw new NullReferenceException("User not found");

        user.GroceryLists = _DBContext.UserGroceryLists.Where(u => u.UserID == id).Select(u => u.GroceryList).ToList();

        return user;
    }

    /// <inheritdoc />
    public User Find(string username)
    {
        var user = _DBContext.Users.Where(u => u.Username == username).FirstOrDefault() ?? throw new NullReferenceException("User not found");

        user.GroceryLists = _DBContext.UserGroceryLists.Where(u => u.UserID == user.Id).Select(u => u.GroceryList).ToList();
        
        return user;
    }

    /// <inheritdoc />
    public User Update(long id, User model)
    {
        User user = _DBContext.Users.Find(id) ?? throw new NullReferenceException("User not found");
        
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