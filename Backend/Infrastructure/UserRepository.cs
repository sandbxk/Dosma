using Domain;
using Infrastructure.Interfaces;

namespace Infrastructure;

public class UserRepository : IRepository<User>
{
    private DatabaseContext _dbcontext;
    
    public UserRepository(DatabaseContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    public List<User> All()
    {
        throw new NotImplementedException();
    }

    public User Create(User t)
    {
        throw new NotImplementedException();
    }

    public User Delete(int id)
    {
        throw new NotImplementedException();
    }

    public User Single(long id)
    {
        throw new NotImplementedException();
    }

    public User Update(long id, User model)
    {
        throw new NotImplementedException();
    }
}