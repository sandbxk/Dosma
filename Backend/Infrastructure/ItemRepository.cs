using System.Reflection;
using Domain;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ItemRepository : IRepository<Item>
{
    private DatabaseContext _dbContext;

    public ItemRepository(DatabaseContext context)
    {
        _dbContext = context;
    }

    public List<Item> All()
    {
        throw new NotImplementedException();
    }

    public Item Create(Item t)
    {
        GroceryList groceryList = _dbContext.GroceryListsTable
            .Include(l => l.Items).ToList()
            .Find(l => l.Id == t.GroceryListId) ?? throw new InvalidOperationException();

        groceryList.Items.Add(t);
        _dbContext.SaveChanges();
        return t;
    }

    public bool Delete(int id)
    {
        var item = _dbContext.ItemTable.Find(id);
        if (item == null)
        {
            throw new NullReferenceException("Item not found.");
        }
        
        _dbContext.Remove(item);
        int change = _dbContext.SaveChanges();
        
        if (change == 0)
        {
            throw new NullReferenceException("Item not found.");
        }
        return true;
    }

    public Item Single(long id)
    {
        throw new NotImplementedException();
    }

    public Item Update(Item model)
    {
        var existingItem = _dbContext.ItemTable.Find(model.Id) ?? throw new NullReferenceException("Item not found.");

        foreach (PropertyInfo prop in existingItem.GetType().GetProperties())
        {
            var PropertyName = prop.Name;
            var value = prop.GetValue(model);
            
            if (value != null)
            {
                existingItem.GetType().GetProperty(PropertyName).SetValue(existingItem, value);
            }
        }
        
        _dbContext.Update(existingItem);
        int change = _dbContext.SaveChanges();

        if (change == 0)
        {
            throw new NullReferenceException("Unable to update item.");
        }
        return model;
    }
}
