using System.ComponentModel.DataAnnotations;
using System.Reflection;
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
    
    public List<GroceryList> All()
    {
        return _dbContext.GroceryListsTable
            .Include(l => l.Items)
            .Include(l => l.Users)
            .ToList();
    }

    public GroceryList Create(GroceryList t)
    {
        if (t.Items == null)
        {
            t.Items = new List<Item>();
        }
        
        _dbContext.GroceryListsTable.Add(t);
        int change = _dbContext.SaveChanges();

        if (change == 0)
        {
            throw new Exception("Unable to create list");
        }
        return t;
    }

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

    public GroceryList Single(long id)
    {
        throw new NotImplementedException();
    }

    public GroceryList Update(GroceryList model)
    {
        var existingList = _dbContext.GroceryListsTable.Find(model.Id) ?? throw new NullReferenceException("List not found.");

        foreach (PropertyInfo prop in existingList.GetType().GetProperties())
        {
            var propertyName = prop.Name;
            var value = prop.GetValue(model);
            
            if (value != null)
            {
                var field = existingList.GetType().GetProperty(propertyName);
                if (field != null)
                {
                    field.SetValue(existingList, value);
                }
            }
        }
        
        _dbContext.Update(existingList);
        int change = _dbContext.SaveChanges();

        if (change == 0)
        {
            throw new NullReferenceException("Unable to update list.");
        }
        return model;
    }

    public List<UserList> AddUser(User user, GroceryList groceryList)
    {
        throw new NotImplementedException();
    }

    public List<UserList> RemoveUser(User user, GroceryList groceryList)
    {
        throw new NotImplementedException();
    }
}