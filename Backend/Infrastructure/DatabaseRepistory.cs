using Infrastructure.Interfaces;
using SQLitePCL;

namespace Infrastructure;

public class DatabaseRepistory : IDatabase
{
    private DatabaseContext _dbcontext;
    
    public DatabaseRepistory(DatabaseContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    public void BuildDB()
    {
        _dbcontext.Database.EnsureDeleted();
        _dbcontext.Database.EnsureCreated();
    }
}