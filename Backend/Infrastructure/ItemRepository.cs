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
        var groceryList = _dbContext.GroceryListsTable.Single(l => l.Id == t.GroceryListId);
        var newItem = new Item
        {
            Title = t.Title,
            GroceryList = groceryList
        };
        
        _dbContext.ItemTable.Add(newItem);
        _dbContext.SaveChanges();
        return newItem;
    }

    public Item Delete(int id)
    {
        var item = _dbContext.ItemTable.Find(id);
        _dbContext.Remove(item);
        _dbContext.SaveChanges();
        return item;
    }

    public Item Single(long id)
    {
        throw new NotImplementedException();
    }

    public Item Update(long id, Item model)
    {
        throw new NotImplementedException();
    }
}