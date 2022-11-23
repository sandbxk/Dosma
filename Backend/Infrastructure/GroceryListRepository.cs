using Domain;
using Infrastructure.Interfaces;

namespace Infrastructure;

public class GroceryListRepository : IRepository<GroceryList>
{
    private DatabaseContext _dbContext;
    
    public GroceryListRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IEnumerable<GroceryList> All()
    {
        return _dbContext.GroceryListsTable.ToList();
    }

    public GroceryList Create(GroceryList t)
    {
        _dbContext.GroceryListsTable.Add(t);
        return t;
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<GroceryList> SearchByName(string tName)
    {
        throw new NotImplementedException();
    }

    public GroceryList Single(long id)
    {
        throw new NotImplementedException();
    }

    public GroceryList Update(long id, GroceryList model)
    {
        throw new NotImplementedException();
    }

    public void BuildDB()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
    }
}