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
        var existingList =  _dbContext.GroceryListsTable
            .Include(l => l.Items)
            .Include(l => l.SharedList)
            .ToList();

        foreach (var GroceryList in existingList )
        {
            var j = _dbContext.UserGroceryListsTable.Where(j => j.GroceryListID == GroceryList.Id).ToList();

            List<User> user = new List<User>();

            foreach (var UserList in j)
            {
                user.Add(_dbContext.UserTable.Find(UserList.UserID));
            }
            
            GroceryList.Users = user;
        }

        return existingList;
    }
    

    public List<GroceryList> AlternativeAll()
    {
         var gl = _dbContext.GroceryListsTable.ToList();

        foreach (var groceryList in gl)
        {
            groceryList.Items = _dbContext.ItemTable.Where(i => i.GroceryListId == groceryList.Id).ToList();
            groceryList.Users = _dbContext.UserGroceryListsTable.Where(u => u.GroceryListID == groceryList.Id).Select(u => u.User).ToList();
        }

        return gl;
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
        return _dbContext.GroceryListsTable
            .Include(l => l.Items)
            .FirstOrDefault(l => l.Id == id);
    }

    /// <inheritdoc />
    public GroceryList Update(GroceryList model)
    {
        _dbContext.GroceryListsTable.Update(model);
        _dbContext.SaveChanges();
        return model;
    }
}