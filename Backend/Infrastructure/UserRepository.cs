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

    public bool Delete(int id)
    {
        User user = _DBContext.UserTable.Find(id) ?? throw new NullReferenceException("User not found");

        _DBContext.UserTable.Remove(user);
        int change = _DBContext.SaveChanges();
        
        if (change == 0)
        {
            return false;
        }

        return true;
    }

    public User Single(int id)
    {
        return _DBContext.UserTable.Find(id) ?? throw new NullReferenceException("User does not exist");
    }

    public User Find(string username)
    {
        return _DBContext.UserTable.Where(u => u.Username == username).FirstOrDefault() ?? throw new NullReferenceException("User not found");
    }

    public User Update(long id, User model)
    {
        User user = _DBContext.UserTable.Find(id) ?? throw new NullReferenceException("User not found");
        
        user.Username = model.Username;
        user.HashedPassword = model.HashedPassword;
        user.DisplayName = model.DisplayName;
        user.SharedList = model.SharedList;
        _DBContext.SaveChanges();
        return user;
    }

    public User Update(User model)
    {
        return this.Update(model.Id, model);
    }
}