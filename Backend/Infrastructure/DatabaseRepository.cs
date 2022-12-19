using Infrastructure.Interfaces;
using SQLitePCL;

namespace Infrastructure;

public class DatabaseRepository : IDatabase
{
    private DatabaseContext _dbcontext;
    
    public DatabaseRepository(DatabaseContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    public void BuildDB()
    {
        _dbcontext.Database.EnsureDeleted();
        _dbcontext.Database.EnsureCreated();
    }
}