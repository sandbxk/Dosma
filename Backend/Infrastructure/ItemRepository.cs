using System.Reflection;
using Backend.Infrastructure;
using Domain;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ItemRepository : IItemRepository
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

    /// <inheritdoc />
    public int BulkCreate(List<Item> t)
    {
        t.ForEach(item => {
            var groceryList = _dbContext.GroceryListsTable
                .Include(l => l.Items).ToList()
                .Find(l => l.Id == item.GroceryListId) ?? throw new InvalidOperationException();
            groceryList.Items.Add(item);
        });

        return _dbContext.SaveChanges();
    }

    /// <inheritdoc />
    public int BulkDelete(List<Item> t)
    {
        t.ForEach(item => {
            var groceryList = _dbContext.GroceryListsTable
                .Include(l => l.Items).ToList()
                .Find(l => l.Id == item.GroceryListId) ?? throw new InvalidOperationException();
            groceryList.Items.Remove(item);
        });

        return _dbContext.SaveChanges();
    }

    /// <inheritdoc />
    public int BulkUpdate(List<Item> t)
    {
        t.ForEach(item => {
            var existingItem = _dbContext.ItemTable.Find(item.Id);

            if (existingItem == null)
            {
                return;
            }

            foreach (PropertyInfo prop in existingItem.GetType().GetProperties())
            {
                var propertyName = prop.Name;
                var value = prop.GetValue(item);
                
                if (value != null)
                {
                    var field = existingItem.GetType().GetProperty(propertyName);
                    if (field != null)
                    {
                        field.SetValue(existingItem, value);
                    }
                }
            }
            
            _dbContext.Update(existingItem);
        });

        return _dbContext.SaveChanges();
    }

    /// <inheritdoc />
    public Item Create(Item t)
    {
        GroceryList groceryList = _dbContext.GroceryListsTable
            .Include(l => l.Items).ToList()
            .Find(l => l.Id == t.GroceryListId) ?? throw new InvalidOperationException();

        groceryList.Items.Add(t);
        _dbContext.SaveChanges();
        return t;
    }

    /// <inheritdoc />
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
            return false;
        }
        return true;
    }

    /// <inheritdoc />
    public List<Item> GetAllForList(int id)
    {
        return _dbContext.ItemTable.Where(i => i.GroceryListId == id).ToList();
    }

    /// <inheritdoc />
    public Item Single(long id)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Item Update(Item model)
    {
        var existingItem = _dbContext.ItemTable.Find(model.Id) ?? throw new NullReferenceException("Item not found.");

        foreach (PropertyInfo prop in existingItem.GetType().GetProperties())
        {
            var propertyName = prop.Name;
            var value = prop.GetValue(model);
            
            if (value != null)
            {
                var field = existingItem.GetType().GetProperty(propertyName);
                if (field != null)
                {
                    field.SetValue(existingItem, value);
                }
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
