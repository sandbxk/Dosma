using System.ComponentModel.DataAnnotations;
using Domain;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class GroceryListRepository : IGroceryListRepository
{
    private DatabaseContext _dbContext;
    
    public GroceryListRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <inheritdoc />
    public List<GroceryList> All()
    {
        return _dbContext.GroceryListsTable
            .Include(l => l.Items)
            .ToList();
    }

    /// <inheritdoc />
    public GroceryList Create(GroceryList t)
    {
        if (t.Items == null)
        {
            t.Items = new List<Item>();
        }
        
        _dbContext.GroceryListsTable.Add(t);
        _dbContext.SaveChanges();
        return t;
    }

    /// <inheritdoc />
    public bool Delete(int id)
    {
        var groceryList = _dbContext.GroceryListsTable.Find(id);

        if (groceryList == null)
        {
            throw new NullReferenceException("List not found.");
        }

        _dbContext.GroceryListsTable.Remove(groceryList);
        int change = _dbContext.SaveChanges();
        
        if (change == 0)
        {
            throw new NullReferenceException("List not found.");
        }
        return true;
    }

    /// <inheritdoc />
    public List<GroceryList> GetAllForUser(int id)
    {
        return _dbContext.GroceryListsTable
            .Include(l => l.Items)
            .ToList();
    }

    /// <inheritdoc />
    public GroceryList Single(long id)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public GroceryList Update(GroceryList model)
    {
        _dbContext.GroceryListsTable.Update(model);
        _dbContext.SaveChanges();
        return model;
    }
}